using AskAQuestion.Questions;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace AskAQuestion.Entrypoints;

/// <summary>
/// Send the full commandline as question to the configured <see cref="IAnswerProvider"/>.
/// </summary>
public sealed class AskQuestion(IServiceProvider serviceProvider) : IEntrypoint
{
    /// <inheritdoc/>
    public bool Accepts(string[] args) => true;

    /// <inheritdoc/>
    public async Task<int> RunAsync(string[] args)
    {
        // TODO: Injecting this eager crashes *every* invocation when no connection string is set.
        // This... is a problem. Huh. Gotta solve this differently.
        var answerProvider = serviceProvider.GetRequiredService<IAnswerProvider>();

        var question = string.Join(' ', args);
        var answer = await answerProvider.GetAnswerAsync(question, CancellationToken.None);

        AnsiConsole.WriteLine(answer);
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[Grey]Note: AI-generated answers might be complete bullshit. Use at your own discretion.[/]");

        return ExitCodes.Success;
    }
}
