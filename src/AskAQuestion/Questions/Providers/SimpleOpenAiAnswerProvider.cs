using System.ClientModel;
using System.ClientModel.Primitives;
using AskAQuestion.Config;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Chat;
using ChatFinishReason = Microsoft.Extensions.AI.ChatFinishReason;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace AskAQuestion.Questions.Providers;

/// <summary>
/// An <see cref="IAnswerProvider"/> based on an OpenAI compatible endpoint.
/// </summary>
public sealed class SimpleOpenAiAnswerProvider : IAnswerProvider
{
    private const string MainSystemPrompt = """
        You are a search engine.
        Answer the questions in the shortest way possible.
        Do not engage with the user, do not be friendly.
        Do not respond with code unless explicitly asked to.
        """;

    private static readonly ChatOptions chatOptions = new()
    {
        Reasoning = new ReasoningOptions { Effort = ReasoningEffort.None },
        Temperature = 0.25f,
        TopP = 0.1f
    };

    private readonly IChatClient chatClient;

    /// <summary>
    /// Create a new instance of the <see cref="SimpleOpenAiAnswerProvider"/>.
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    public SimpleOpenAiAnswerProvider(ModelConnectionString connectionString)
    {
        var openAiClient = new ChatClient(
            model: connectionString.Model,
            credential: new ApiKeyCredential(connectionString.ApiKey ?? "not-used"),
            options: new OpenAIClientOptions
            {
                Endpoint = connectionString.Endpoint,
                RetryPolicy = new ClientRetryPolicy(maxRetries: 0)
            });

        chatClient = openAiClient.AsIChatClient();
    }

    /// <inheritdoc />
    public async Task<string> GetAnswerAsync(string question, CancellationToken cancellationToken)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, MainSystemPrompt),
            new(ChatRole.User, question),
        };

        var response = await chatClient.GetResponseAsync(messages, chatOptions, cancellationToken);
        if (response.FinishReason != ChatFinishReason.Stop)
        {
            throw new InvalidOperationException($"Model finished with unexpected finish reason {response.FinishReason}");
        }

        return response.Text;
    }

    public void Dispose()
    {
        chatClient.Dispose();
    }
}
