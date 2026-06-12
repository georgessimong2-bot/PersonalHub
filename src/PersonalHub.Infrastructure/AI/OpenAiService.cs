using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using PersonalHub.Application.Common.Interfaces;

namespace PersonalHub.Infrastructure.AI;

public class OpenAiService : IAiService
{
    private readonly ChatClient _client;

    public OpenAiService(
        IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"];

        Console.WriteLine("OPENAI KEY FOUND = " +
            !string.IsNullOrWhiteSpace(apiKey));

        Console.WriteLine("OPENAI KEY LENGTH = " +
            apiKey?.Length);

        Console.WriteLine(apiKey);

        Console.WriteLine("OPENAI KEY START = " +
            apiKey?.Substring(0, Math.Min(15, apiKey.Length)));

        _client =
            new ChatClient(
                model: "gpt-4.1-mini",
                apiKey: apiKey);
    }

    public async Task<string> GenerateGoalAdviceAsync(
        string prompt)
    {
        Console.WriteLine("OPENAI CALL");

        var response =
            await _client.CompleteChatAsync(prompt);

        Console.WriteLine("OPENAI RESPONSE RECEIVED");

        return response.Value.Content[0].Text;
    }
}