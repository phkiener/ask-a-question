using AskAQuestion;
using AskAQuestion.Config;
using AskAQuestion.Entrypoints;
using AskAQuestion.Questions;
using Microsoft.Extensions.DependencyInjection;

await using var serviceProvider = new ServiceCollection()
    .AddConfiguration()
    .AddEntrypoints()
    .AddAnswerProviders()
    .BuildServiceProvider();

var features = serviceProvider.GetServices<IEntrypoint>();
var matchingFeature = features.First(f => f.Accepts(args));

try
{
    return await matchingFeature.RunAsync(args);
}
catch (Exception e)
{
    Console.WriteLine(e.Message);

    return ExitCodes.Error;
}
