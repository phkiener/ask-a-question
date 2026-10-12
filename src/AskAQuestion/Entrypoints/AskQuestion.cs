using AskAQuestion.Questions;

namespace AskAQuestion.Entrypoints;

/// <summary>
/// Send the full commandline as question to the configured <see cref="IAnswerProvider"/>.
/// </summary>
public sealed class AskQuestion(IAnswerProvider answerProvider) : IEntrypoint
{
    /// <inheritdoc/>
    public bool Accepts(string[] args) => true;

    /// <inheritdoc/>
    public async Task<int> RunAsync(string[] args)
    {
        var question = string.Join(' ', args);
        var answer = await answerProvider.GetAnswerAsync(question, CancellationToken.None);

        Console.WriteLine(answer);
        Console.WriteLine();
        Console.WriteLine("Note: AI-generated answers might be complete bullshit. Use at your own discretion.");

        return ExitCodes.Success;
    }
}
