using System.Reflection;
using System.Text;
using OfficeOpenXml;
using PersonalHub.Application.Features.BenchmarkPrices.Common;
using PersonalHub.Application.Features.Benchmarks.Common;
using PersonalHub.Application.Features.Funds.Common;
using PersonalHub.Application.Features.Portfolios.Common;
using PersonalHub.Application.Features.ShareClasses.Common;
using PersonalHub.Application.Features.SubFunds.Common;
using PersonalHub.Web.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PersonalHub.Web.Services.Reporting;

public class ReportingService
{
    private static readonly IReadOnlyList<ReportDefinitionDto> Definitions =
    [
        new(
            "fund-overview",
            "Fund Overview Report",
            "Funds",
            "All funds with their type, base currency, domicile country, launch date and status.",
            ["Funds", "Fund Types"]),
        new(
            "subfund-shareclass",
            "SubFund & ShareClass Report",
            "Funds",
            "Sub funds and their share classes with fees, ISIN and classification details.",
            ["Sub Funds", "Share Classes"]),
        new(
            "benchmark-comparison",
            "Benchmark Comparison Report",
            "Benchmarks",
            "Benchmarks catalogue and the sub funds that reference each benchmark.",
            ["Benchmarks", "Sub Funds by Benchmark"]),
        ];

    private readonly FundService _fundService;
    private readonly SubFundService _subFundService;
    private readonly ShareClassService _shareClassService;
    private readonly PortfolioService _portfolioService;
    private readonly PortfolioHoldingService _portfolioHoldingService;
    private readonly FundTypeService _fundTypeService;
    private readonly BenchmarkService _benchmarkService;
    private readonly BenchmarkPriceService _benchmarkPriceService;

    public ReportingService(
        FundService fundService,
        SubFundService subFundService,
        ShareClassService shareClassService,
        PortfolioService portfolioService,
        PortfolioHoldingService portfolioHoldingService,
        FundTypeService fundTypeService,
        BenchmarkService benchmarkService,
        BenchmarkPriceService benchmarkPriceService)
    {
        _fundService = fundService;
        _subFundService = subFundService;
        _shareClassService = shareClassService;
        _portfolioService = portfolioService;
        _portfolioHoldingService = portfolioHoldingService;
        _fundTypeService = fundTypeService;
        _benchmarkService = benchmarkService;
        _benchmarkPriceService = benchmarkPriceService;
    }

    public Task<IReadOnlyList<ReportDefinitionDto>> GetReportsAsync()
        => Task.FromResult(Definitions);

    public async Task<byte[]> GenerateExcelAsync(string reportKey)
    {
        var report = await BuildReportAsync(reportKey);

        ExcelPackage.License.SetNonCommercialPersonal("Georges Simon");

        using var package = new ExcelPackage();

        foreach (var dataset in report.Datasets)
        {
            WriteWorksheet(package, dataset);
        }

        return package.GetAsByteArray();
    }

    public async Task<byte[]> GeneratePdfAsync(string reportKey)
    {
        var report = await BuildReportAsync(reportKey);

        using var stream = new MemoryStream();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Header()
                    .Column(column =>
                    {
                        column.Spacing(4);
                        column.Item().Text(report.Title).SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);
                        column.Item().Text(report.Description).FontColor(Colors.Grey.Darken1);
                    });

                page.Content()
                    .Column(column =>
                    {
                        column.Spacing(16);

                        column.Item().Text("Dataset summary").SemiBold().FontSize(14);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.ConstantColumn(60);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellHeader).Text("Dataset");
                                header.Cell().Element(CellHeader).AlignRight().Text("Rows");
                            });

                            foreach (var item in report.SummaryItems)
                            {
                                table.Cell().Element(CellBody).Text(item.Label);
                                table.Cell().Element(CellBody).AlignRight().Text(item.Value.ToString());
                            }
                        });

                        column.Item().Text("Distribution graph").SemiBold().FontSize(14);

                        column.Item().Column(graph =>
                        {
                            graph.Spacing(6);

                            var max = Math.Max(1, report.SummaryItems.Max(x => x.Value));

                            foreach (var item in report.SummaryItems.OrderByDescending(x => x.Value))
                            {
                                var barWidth = Math.Max(18, (int)Math.Round(220d * item.Value / max));

                                graph.Item().Row(row =>
                                {
                                    row.ConstantItem(120).AlignLeft().PaddingTop(1).Text(item.Label);
                                    row.ConstantItem(barWidth).Height(14).Background(Colors.Blue.Medium);
                                    row.ConstantItem(50).AlignRight().PaddingTop(1).Text(item.Value.ToString());
                                });
                            }
                        });

                        // Time-series sections (one SVG line chart per benchmark / portfolio series)
                        foreach (var section in report.TimeSeriesSections)
                        {
                            column.Item().Text(section.Title).SemiBold().FontSize(13);
                            column.Item().AspectRatio(500f / 180f).Svg(_ => BuildSvgLineChart(section.Entries));
                        }

                    });

                page.Footer()
                    .AlignRight()
                    .Text(x =>
                    {
                        x.Span("Generated ");
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        })
        .GeneratePdf(stream);

        return stream.ToArray();
    }

    private static string BuildSvgLineChart(IReadOnlyList<TimeSeriesEntry> entries)
    {
        var ic = System.Globalization.CultureInfo.InvariantCulture;

        // Sample down to 60 points max
        var step    = Math.Max(1, entries.Count / 60);
        var sampled = entries.Where((_, i) => i % step == 0 || i == entries.Count - 1).ToList();

        if (sampled.Count < 2)
            return "<svg xmlns='http://www.w3.org/2000/svg'/>";

        const float vbW = 500f;
        const float vbH = 180f;
        const float padL = 62f, padR = 12f, padT = 10f, padB = 38f;
        var w = vbW - padL - padR;
        var h = vbH - padT - padB;

        var minVal   = sampled.Min(e => (float)e.Value);
        var maxVal   = sampled.Max(e => (float)e.Value);
        var valRange = maxVal - minVal;
        if (valRange == 0f) valRange = 1f;

        var minDate  = sampled.Min(e => e.Date);
        var maxDate  = sampled.Max(e => e.Date);
        var dayRange = (float)(maxDate - minDate).TotalDays;
        if (dayRange == 0f) dayRange = 1f;

        string Px(float v)  => v.ToString("F1", ic);
        float  X(DateTime d) => padL + (float)(d - minDate).TotalDays / dayRange * w;
        float  Y(float v)    => padT + h - (v - minVal) / valRange * h;

        var sb = new StringBuilder();
        sb.Append($"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 {Px(vbW)} {Px(vbH)}'>");

        // Grid lines + Y labels
        const int gridCount = 5;
        for (var i = 0; i <= gridCount; i++)
        {
            var frac = (float)i / gridCount;
            var gy   = padT + h - frac * h;
            var gval = minVal + valRange * frac;

            var dashAttr = i == 0 ? "" : " stroke-dasharray='4 4'";
            sb.Append($"<line x1='{Px(padL)}' y1='{Px(gy)}' x2='{Px(padL + w)}' y2='{Px(gy)}' stroke='#d0d0d0' stroke-width='0.6'{dashAttr}/>");

            var label = gval >= 1_000_000 ? (gval / 1_000_000).ToString("F1", ic) + "M"
                      : gval >= 1_000     ? (gval / 1_000).ToString("F1", ic) + "K"
                      : gval.ToString("N0", ic);
            sb.Append($"<text x='{Px(padL - 4)}' y='{Px(gy + 3)}' text-anchor='end' font-size='8' fill='#888'>{label}</text>");
        }

        // Y axis
        sb.Append($"<line x1='{Px(padL)}' y1='{Px(padT)}' x2='{Px(padL)}' y2='{Px(padT + h)}' stroke='#aaa' stroke-width='1'/>");

        // X axis date labels (max 8)
        var xStep = Math.Max(1, sampled.Count / 8);
        for (var i = 0; i < sampled.Count; i += xStep)
        {
            var lx   = X(sampled[i].Date);
            var text = sampled[i].Date.ToString("MMM yy", ic);
            sb.Append($"<line x1='{Px(lx)}' y1='{Px(padT + h)}' x2='{Px(lx)}' y2='{Px(padT + h + 4)}' stroke='#aaa' stroke-width='1'/>");
            sb.Append($"<text x='{Px(lx)}' y='{Px(padT + h + 14)}' text-anchor='middle' font-size='8' fill='#888'>{text}</text>");
        }

        // Filled area under the line
        var fillPath = new StringBuilder();
        fillPath.Append($"M {Px(X(sampled[0].Date))} {Px(padT + h)}");
        foreach (var e in sampled)
            fillPath.Append($" L {Px(X(e.Date))} {Px(Y((float)e.Value))}");
        fillPath.Append($" L {Px(X(sampled[^1].Date))} {Px(padT + h)} Z");
        sb.Append($"<path d='{fillPath}' fill='rgba(25,118,210,0.12)'/>");

        // Line
        var linePath = new StringBuilder();
        linePath.Append($"M {Px(X(sampled[0].Date))} {Px(Y((float)sampled[0].Value))}");
        for (var i = 1; i < sampled.Count; i++)
            linePath.Append($" L {Px(X(sampled[i].Date))} {Px(Y((float)sampled[i].Value))}");
        sb.Append($"<path d='{linePath}' fill='none' stroke='#1976D2' stroke-width='2' stroke-linejoin='round' stroke-linecap='round'/>");

        // Dots (only when not too crowded)
        if (sampled.Count <= 30)
        {
            foreach (var e in sampled)
            {
                var cx = Px(X(e.Date));
                var cy = Px(Y((float)e.Value));
                sb.Append($"<circle cx='{cx}' cy='{cy}' r='3.5' fill='#1976D2'/>");
                sb.Append($"<circle cx='{cx}' cy='{cy}' r='3.5' fill='none' stroke='white' stroke-width='1.5'/>");
            }
        }

        sb.Append("</svg>");
        return sb.ToString();
    }

    private static IContainer CellHeader(IContainer container)
    {
        return container
            .Background(Colors.Grey.Lighten3)
            .PaddingVertical(6)
            .PaddingHorizontal(8)
            .DefaultTextStyle(x => x.SemiBold());
    }

    private static IContainer CellBody(IContainer container)
    {
        return container
            .BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingVertical(5)
            .PaddingHorizontal(8);
    }

    private async Task<ReportBundle> BuildReportAsync(string reportKey)
    {
        return reportKey switch
        {
            "fund-overview"        => await BuildFundOverviewAsync(),
            "subfund-shareclass"   => await BuildSubFundShareClassAsync(),
            "benchmark-comparison" => await BuildBenchmarkComparisonAsync(),
            _ => throw new ArgumentOutOfRangeException(nameof(reportKey), reportKey, "Unknown report key.")
        };
    }

    private async Task<ReportBundle> BuildFundOverviewAsync()
    {
        var fundsTask     = _fundService.GetFundsAsync();
        var fundTypesTask = _fundTypeService.GetFundTypesAsync();

        await Task.WhenAll(fundsTask, fundTypesTask);

        var fundRows = (await fundsTask)
            .Select(f => new FundReportRow(
                f.Name,
                f.LegalName,
                f.FundCode,
                f.FundTypeName,
                f.DomicileCountry,
                f.BaseCurrency,
                f.LaunchDate,
                f.IsActive,
                f.SubFundCount,
                f.Description))
            .Cast<object>()
            .ToList();

        var fundTypeRows = (await fundTypesTask).Cast<object>().ToList();

        return CreateBundle(
            "Fund Overview Report",
            "All funds with their type, base currency, domicile country, launch date and status.",
            [new("Funds", fundRows.Count), new("Fund Types", fundTypeRows.Count)],
            [new("Funds", fundRows), new("Fund Types", fundTypeRows)]);
    }

    private async Task<ReportBundle> BuildSubFundShareClassAsync()
    {
        var subFundsTask    = _subFundService.GetSubFundsAsync();
        var shareClassesTask = _shareClassService.GetShareClassesAsync();

        await Task.WhenAll(subFundsTask, shareClassesTask);

        var subFunds    = (await subFundsTask).ToList();
        var shareClasses = (await shareClassesTask).ToList();

        var subFundMap = subFunds.ToDictionary(sf => sf.Id, sf => sf.Name);

        var subFundRows = subFunds.Cast<object>().ToList();

        var shareClassRows = shareClasses
            .Select(sc => new ShareClassReportRow(
                sc.Name,
                sc.ISIN,
                subFundMap.GetValueOrDefault(sc.SubFundId, sc.SubFundId.ToString()),
                sc.IsHedged,
                sc.IsDistribution,
                sc.IsInstitutional,
                sc.ManagementFee,
                sc.PerformanceFee,
                sc.MinimumInvestment,
                sc.LaunchDate,
                sc.IsActive))
            .Cast<object>()
            .ToList();

        return CreateBundle(
            "SubFund & ShareClass Report",
            "Sub funds and their share classes with fees, ISIN and classification details.",
            [new("Sub Funds", subFundRows.Count), new("Share Classes", shareClassRows.Count)],
            [new("Sub Funds", subFundRows), new("Share Classes", shareClassRows)]);
    }

    private async Task<ReportBundle> BuildBenchmarkComparisonAsync()
    {
        var benchmarksTask = _benchmarkService.GetBenchmarksAsync();
        var subFundsTask   = _subFundService.GetSubFundsAsync();

        await Task.WhenAll(benchmarksTask, subFundsTask);

        var benchmarks = (await benchmarksTask).ToList();
        var subFunds   = (await subFundsTask).ToList();

        var benchmarkMap = benchmarks.ToDictionary(b => b.Id, b => b.Name);

        var benchmarkRows = benchmarks
            .Select(b => new BenchmarkReportRow(
                b.Name,
                b.BloombergTicker,
                b.ReutersCode,
                b.Provider,
                b.Currency?.Code,
                b.IsActive,
                subFunds.Count(sf => sf.BenchmarkId == b.Id)))
            .Cast<object>()
            .ToList();

        var subFundBenchmarkRows = subFunds
            .Where(sf => sf.BenchmarkId.HasValue)
            .Select(sf => new SubFundBenchmarkRow(
                sf.Name,
                sf.InternalCode,
                sf.GeographicFocus,
                sf.SectorFocus,
                benchmarkMap.GetValueOrDefault(sf.BenchmarkId!.Value, sf.BenchmarkId.Value.ToString())))
            .Cast<object>()
            .ToList();

        // Fetch price history for each benchmark in parallel
        var priceTasks = benchmarks.Select(async b =>
        {
            var prices = await _benchmarkPriceService.GetBenchmarkPricesAsync(b.Id);
            return (b.Name, Prices: prices.OrderBy(p => p.PriceDate).ToList());
        }).ToList();

        await Task.WhenAll(priceTasks);

        var timeSeriesSections = priceTasks
            .Select(t => t.Result)
            .Where(r => r.Prices.Count >= 2)
            .Select(r => new TimeSeriesSection(
                $"{r.Name} — Price History",
                r.Prices
                    .Select(p => new TimeSeriesEntry(p.PriceDate, p.Price))
                    .ToList()))
            .ToList();

        return CreateBundle(
            "Benchmark Comparison Report",
            "Benchmarks catalogue and the sub funds that reference each benchmark.",
            [new("Benchmarks", benchmarkRows.Count), new("Sub Funds by Benchmark", subFundBenchmarkRows.Count)],
            [new("Benchmarks", benchmarkRows), new("Sub Funds by Benchmark", subFundBenchmarkRows)],
            timeSeriesSections);
    }

    private static ReportBundle CreateBundle(
        string title,
        string description,
        IReadOnlyList<ReportSummaryItem> summaryItems,
        IReadOnlyList<ReportDataset> datasets,
        IReadOnlyList<TimeSeriesSection>? timeSeriesSections = null)
    {
        return new ReportBundle(title, description, summaryItems, datasets, timeSeriesSections ?? []);
    }

    private static void WriteWorksheet(ExcelPackage package, ReportDataset dataset)
    {
        var worksheet = package.Workbook.Worksheets.Add(dataset.SheetName);

        if (dataset.Rows.Count == 0)
        {
            worksheet.Cells[1, 1].Value = "No data";
            worksheet.Cells[1, 1].Style.Font.Italic = true;
            return;
        }

        var properties = dataset.Rows[0]
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(property => property.CanRead)
            .ToArray();

        for (var column = 0; column < properties.Length; column++)
        {
            worksheet.Cells[1, column + 1].Value = properties[column].Name;
            worksheet.Cells[1, column + 1].Style.Font.Bold = true;
        }

        for (var rowIndex = 0; rowIndex < dataset.Rows.Count; rowIndex++)
        {
            var row = dataset.Rows[rowIndex];

            for (var columnIndex = 0; columnIndex < properties.Length; columnIndex++)
            {
                var value = properties[columnIndex].GetValue(row);
                worksheet.Cells[rowIndex + 2, columnIndex + 1].Value = FormatCellValue(value);
            }
        }

        worksheet.Cells.AutoFitColumns();
    }

    private static object? FormatCellValue(object? value)
    {
        return value switch
        {
            null => null,
            DateTime dateTime => dateTime.ToString("yyyy-MM-dd"),
            DateOnly dateOnly => dateOnly.ToString("yyyy-MM-dd"),
            DateTimeOffset dateTimeOffset => dateTimeOffset.ToString("yyyy-MM-dd"),
            bool boolean => boolean ? "Yes" : "No",
            Guid guid => guid.ToString(),
            Enum enumeration => enumeration.ToString(),
            decimal decimalValue => decimalValue,
            double doubleValue => doubleValue,
            float floatValue => floatValue,
            _ => value.ToString()
        };
    }

    private sealed record ReportBundle(
        string Title,
        string Description,
        IReadOnlyList<ReportSummaryItem> SummaryItems,
        IReadOnlyList<ReportDataset> Datasets,
        IReadOnlyList<TimeSeriesSection> TimeSeriesSections);

    private sealed record TimeSeriesSection(
        string Title,
        IReadOnlyList<TimeSeriesEntry> Entries);

    private sealed record TimeSeriesEntry(
        DateTime Date,
        decimal Value);

    private sealed record ReportDataset(
        string SheetName,
        IReadOnlyList<object> Rows);

    private sealed record ReportSummaryItem(
        string Label,
        int Value);

    // Fund Overview
    private sealed record FundReportRow(
        string Name,
        string? LegalName,
        string? FundCode,
        string FundTypeName,
        string? DomicileCountry,
        string? BaseCurrency,
        DateTime? LaunchDate,
        bool IsActive,
        int SubFundCount,
        string? Description);

    // SubFund & ShareClass
    private sealed record ShareClassReportRow(
        string Name,
        string ISIN,
        string SubFundName,
        bool IsHedged,
        bool IsDistribution,
        bool IsInstitutional,
        decimal? ManagementFee,
        decimal? PerformanceFee,
        decimal? MinimumInvestment,
        DateTime? LaunchDate,
        bool IsActive);

    // Benchmark Comparison
    private sealed record BenchmarkReportRow(
        string Name,
        string? BloombergTicker,
        string? ReutersCode,
        string? Provider,
        string? CurrencyCode,
        bool IsActive,
        int SubFundsUsingCount);

    private sealed record SubFundBenchmarkRow(
        string SubFundName,
        string? InternalCode,
        string? GeographicFocus,
        string? SectorFocus,
        string BenchmarkName);

    // Portfolio Summary
    private sealed record PortfolioReportRow(
        string FundName,
        string SubFundName,
        string ShareClassName,
        DateTime ValuationDate,
        bool IsActive,
        int HoldingsCount,
        decimal? TotalMarketValue);

    private sealed record PortfolioHoldingReportRow(
        string FundName,
        string SubFundName,
        string ShareClassName,
        DateTime ValuationDate,
        string InstrumentName,
        string InstrumentISIN,
        decimal Quantity,
        decimal? AverageCost,
        decimal? MarketValue);
}

public sealed record ReportDefinitionDto(
    string Key,
    string Name,
    string Category,
    string Description,
    IReadOnlyList<string> DatasetNames);
