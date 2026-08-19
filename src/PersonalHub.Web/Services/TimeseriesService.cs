using PersonalHub.Application.Features.BenchmarkPrices.Common;
using PersonalHub.Application.Features.InstrumentPrices.Common;
using PersonalHub.Application.Features.Portfolios.Common;

namespace PersonalHub.Web.Services;

/// <summary>
/// Service pour gérer les données de séries chronologiques (timeseries) pour la comparaison
/// entre benchmarks et portefeuilles.
/// </summary>
public class TimeseriesService
{
    private readonly BenchmarkPriceService _benchmarkPriceService;
    private readonly PortfolioService _portfolioService;
    private readonly InstrumentPriceService _instrumentPriceService;
    private readonly ILogger<TimeseriesService> _logger;

    public TimeseriesService(
        BenchmarkPriceService benchmarkPriceService,
        PortfolioService portfolioService,
        InstrumentPriceService instrumentPriceService,
        ILogger<TimeseriesService> logger)
    {
        _benchmarkPriceService = benchmarkPriceService;
        _portfolioService = portfolioService;
        _instrumentPriceService = instrumentPriceService;
        _logger = logger;
    }

    /// <summary>
    /// Récupère les données de prix du benchmark.
    /// </summary>
    public async Task<List<TimeseriesDataPoint>> GetBenchmarkTimeseriesAsync(Guid benchmarkId)
    {
        try
        {
            var prices = await _benchmarkPriceService.GetBenchmarkPricesAsync(benchmarkId);
            return prices
                .OrderBy(p => p.PriceDate)
                .Select(p => new TimeseriesDataPoint
                {
                    Date = p.PriceDate,
                    Value = p.Price,
                    Label = p.PriceDate.ToShortDateString()
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching benchmark timeseries for benchmark {BenchmarkId}", benchmarkId);
            return new List<TimeseriesDataPoint>();
        }
    }

    /// <summary>
    /// Calcule la valeur agrégée de tous les portefeuilles d'une ShareClass au fil du temps.
    /// </summary>
    public async Task<List<TimeseriesDataPoint>> GetShareClassTimeseriesAsync(Guid shareClassId)
    {
        try
        {
            // Récupérer les portefeuilles de cette ShareClass uniquement
            var shareClassPortfolios = await _portfolioService.GetPortfoliosAsync(shareClassId);

            _logger.LogInformation("Found {Count} portfolios for ShareClass {ShareClassId}", shareClassPortfolios.Count, shareClassId);

            if (shareClassPortfolios.Count == 0)
            {
                _logger.LogWarning("No portfolios found for ShareClass {ShareClassId}", shareClassId);
                return new List<TimeseriesDataPoint>();
            }

            // Agréger les valeurs de tous les portefeuilles par date de valuation
            var shareClassValueByDate = new Dictionary<DateTime, decimal>();

            foreach (var portfolio in shareClassPortfolios)
            {
                _logger.LogInformation("Processing portfolio {PortfolioId} with ValuationDate {ValuationDate} and TotalMarketValue {TotalMarketValue}", 
                    portfolio.Id, portfolio.ValuationDate, portfolio.TotalMarketValue);

                if (portfolio.TotalMarketValue == null || portfolio.TotalMarketValue <= 0)
                {
                    _logger.LogWarning("Portfolio {PortfolioId} has no market value", portfolio.Id);
                    continue;
                }

                // Ajouter la valeur totale du portefeuille à la date de valuation
                if (!shareClassValueByDate.ContainsKey(portfolio.ValuationDate))
                {
                    shareClassValueByDate[portfolio.ValuationDate] = 0;
                }

                shareClassValueByDate[portfolio.ValuationDate] += portfolio.TotalMarketValue.Value;
                _logger.LogInformation("Added portfolio value {TotalMarketValue} for date {ValuationDate}", 
                    portfolio.TotalMarketValue, portfolio.ValuationDate);
            }

            _logger.LogInformation("Found {DateCount} unique valuation dates for ShareClass", shareClassValueByDate.Count);

            // Convertir en liste de TimeseriesDataPoint triée par date
            var result = shareClassValueByDate
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new TimeseriesDataPoint
                {
                    Date = kvp.Key,
                    Value = kvp.Value,
                    Label = kvp.Key.ToShortDateString()
                })
                .ToList();

            _logger.LogInformation("ShareClass timeseries result: {Count} points", result.Count);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching ShareClass timeseries for ShareClass {ShareClassId}", shareClassId);
            return new List<TimeseriesDataPoint>();
        }
    }

    /// <summary>
    /// Calcule la valeur du portefeuille au fil du temps en fonction des prix historiques des instruments.
    /// </summary>
    public async Task<List<TimeseriesDataPoint>> GetPortfolioTimeseriesAsync(Guid portfolioId)
    {
        try
        {
            var portfolio = await _portfolioService.GetPortfolioByIdAsync(portfolioId);
            if (portfolio == null || portfolio.Holdings == null || portfolio.Holdings.Count == 0)
            {
                _logger.LogWarning("Portfolio {PortfolioId} not found or has no holdings", portfolioId);
                return new List<TimeseriesDataPoint>();
            }

            // Récupérer tous les prix historiques pour les instruments du portefeuille
            var instrumentPricesDict = new Dictionary<Guid, List<InstrumentPriceDto>>();

            foreach (var holding in portfolio.Holdings)
            {
                var prices = await _instrumentPriceService.GetInstrumentPricesAsync(holding.InstrumentId);
                instrumentPricesDict[holding.InstrumentId] = prices;
            }

            // Agréger les valeurs du portefeuille par date
            var portfolioValueByDate = new Dictionary<DateTime, decimal>();

            foreach (var holding in portfolio.Holdings)
            {
                if (instrumentPricesDict.TryGetValue(holding.InstrumentId, out var prices))
                {
                    foreach (var price in prices)
                    {
                        var holdingValue = holding.Quantity * price.Price;

                        if (!portfolioValueByDate.ContainsKey(price.PriceDate))
                        {
                            portfolioValueByDate[price.PriceDate] = 0;
                        }

                        portfolioValueByDate[price.PriceDate] += holdingValue;
                    }
                }
            }

            // Convertir en liste de TimeseriesDataPoint triée par date
            var result = portfolioValueByDate
                .OrderBy(kvp => kvp.Key)
                .Select(kvp => new TimeseriesDataPoint
                {
                    Date = kvp.Key,
                    Value = kvp.Value,
                    Label = kvp.Key.ToShortDateString()
                })
                .ToList();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching portfolio timeseries for portfolio {PortfolioId}", portfolioId);
            return new List<TimeseriesDataPoint>();
        }
    }

    /// <summary>
    /// Normalise deux séries temporelles pour faciliter la comparaison visuelle.
    /// Retourne les indices de performance avec une base 100 à la première date.
    /// </summary>
    public List<TimeseriesDataPoint> NormalizeTimeseries(List<TimeseriesDataPoint> timeseries)
    {
        if (timeseries == null || timeseries.Count == 0)
        {
            return new List<TimeseriesDataPoint>();
        }

        var baseValue = timeseries.FirstOrDefault()?.Value ?? 100;
        if (baseValue == 0)
        {
            baseValue = 100;
        }

        return timeseries.Select(dp => new TimeseriesDataPoint
        {
            Date = dp.Date,
            Value = (dp.Value / baseValue) * 100m,
            Label = dp.Label
        }).ToList();
    }

    /// <summary>
    /// Aligne deux séries temporelles sur les mêmes dates.
    /// Utilise une interpolation linéaire simple pour les dates manquantes.
    /// </summary>
    public (List<TimeseriesDataPoint> benchmark, List<TimeseriesDataPoint> portfolio) AlignTimeseries(
        List<TimeseriesDataPoint> benchmarkData,
        List<TimeseriesDataPoint> portfolioData)
    {
        if (benchmarkData == null || portfolioData == null)
        {
            return (benchmarkData ?? new(), portfolioData ?? new());
        }

        // Obtenir toutes les dates uniques
        var allDates = benchmarkData.Select(d => d.Date)
            .Union(portfolioData.Select(d => d.Date))
            .OrderBy(d => d)
            .ToList();

        if (allDates.Count == 0)
        {
            return (new List<TimeseriesDataPoint>(), new List<TimeseriesDataPoint>());
        }

        var alignedBenchmark = InterpolateTimeseries(benchmarkData, allDates);
        var alignedPortfolio = InterpolateTimeseries(portfolioData, allDates);

        return (alignedBenchmark, alignedPortfolio);
    }

    private List<TimeseriesDataPoint> InterpolateTimeseries(List<TimeseriesDataPoint> data, List<DateTime> targetDates)
    {
        var result = new List<TimeseriesDataPoint>();

        foreach (var date in targetDates)
        {
            var exactMatch = data.FirstOrDefault(d => d.Date.Date == date.Date);
            if (exactMatch != null)
            {
                result.Add(new TimeseriesDataPoint
                {
                    Date = date,
                    Value = exactMatch.Value,
                    Label = date.ToShortDateString()
                });
            }
            else
            {
                // Interpolation linéaire
                var before = data.Where(d => d.Date < date).OrderByDescending(d => d.Date).FirstOrDefault();
                var after = data.Where(d => d.Date > date).OrderBy(d => d.Date).FirstOrDefault();

                decimal interpolatedValue;
                if (before != null && after != null)
                {
                    var totalDays = (after.Date - before.Date).TotalDays;
                    var daysFromBefore = (date - before.Date).TotalDays;
                    var ratio = daysFromBefore / totalDays;
                    interpolatedValue = before.Value + (decimal)(ratio * (double)(after.Value - before.Value));
                }
                else if (before != null)
                {
                    interpolatedValue = before.Value;
                }
                else if (after != null)
                {
                    interpolatedValue = after.Value;
                }
                else
                {
                    continue;
                }

                result.Add(new TimeseriesDataPoint
                {
                    Date = date,
                    Value = interpolatedValue,
                    Label = date.ToShortDateString()
                });
            }
        }

        return result;
    }
}

/// <summary>
/// Représente un point de données dans une série temporelle.
/// </summary>
public class TimeseriesDataPoint
{
    public DateTime Date { get; set; }
    public decimal Value { get; set; }
    public string Label { get; set; } = string.Empty;
}
