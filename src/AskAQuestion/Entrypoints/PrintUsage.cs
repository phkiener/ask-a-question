namespace AskAQuestion.Entrypoints;

/// <summary>
/// Print usage information, i.e. all available features.
/// </summary>
public sealed class PrintUsage : IEntrypoint
{
    /// <inheritdoc/>
    public bool Accepts(string[] args) => args is ["-h"] or ["--help"];

    /// <inheritdoc/>
    public Task<int> RunAsync(string[] args)
    {
        Console.WriteLine("Usage");
        Console.WriteLine("  aaq <question>            Generate an answer for the given question");
        Console.WriteLine("  aaq config $KEY           Get the current value for $KEY");
        Console.WriteLine("  aaq config $KEY $VALUE    Set $VALUE as config for $KEY");
        Console.WriteLine();

        return Task.FromResult(ExitCodes.Success);
    }
}
