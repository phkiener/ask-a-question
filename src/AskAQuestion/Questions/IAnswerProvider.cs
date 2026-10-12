namespace AskAQuestion.Questions;

/// <summary>
/// An answer provider capable of generating an answer to a question.
/// </summary>
public interface IAnswerProvider : IDisposable
{
    Task<string> GetAnswerAsync(string question, CancellationToken cancellationToken);
}
