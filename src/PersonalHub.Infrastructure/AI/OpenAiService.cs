using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using PersonalHub.Application.Common.Interfaces;
using PersonalHub.Application.Features.AI;
using System.Text.Json;

namespace PersonalHub.Infrastructure.AI;

public class OpenAiService : IAiService
{
    private readonly ChatClient _client;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public OpenAiService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new Exception("OpenAI API key is missing");

        _client = new ChatClient(
            model: "gpt-4.1-mini",
            apiKey: apiKey
        );
    }

    public async Task<AiGoalAdvice> GenerateGoalAdviceAsync(string prompt)
    {
        var systemPrompt = """
You are an AI goal coach.

You MUST return ONLY valid JSON.
No markdown. No explanations. No extra text.

Schema:
{
  "summary": "string",
  "keyInsights": ["string"],
  "actions": ["string"],
  "warning": "string or null",
  "confidenceScore": number (0-100)
}

If you cannot comply, return:
{
  "summary": "error",
  "keyInsights": [],
  "actions": [],
  "warning": "Invalid AI response",
  "confidenceScore": 0
}
""";

        var response = await _client.CompleteChatAsync(
        [
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(prompt)
        ]);

        var text = response.Value.Content[0].Text;

        var json = ExtractJson(text);

        var result = JsonSerializer.Deserialize<AiGoalAdvice>(
            json,
            _jsonOptions);

        if (result is null)
        {
            return new AiGoalAdvice
            {
                Summary = "AI parsing failed",
                Warning = "Could not parse AI response",
                ConfidenceScore = 0
            };
        }

        return result;
    }

    private static string ExtractJson(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            throw new Exception("Empty AI response");

        var start = input.IndexOf('{');
        var end = input.LastIndexOf('}');

        if (start == -1 || end == -1 || end <= start)
            throw new Exception("Invalid AI JSON format");

        return input[start..(end + 1)];
    }
}