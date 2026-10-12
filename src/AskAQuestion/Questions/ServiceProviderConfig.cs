using AskAQuestion.Config;
using AskAQuestion.Questions.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace AskAQuestion.Questions;

/// <summary>
/// DI registration for a <see cref="IAnswerProvider"/>s.
/// </summary>
public static class ServiceProviderConfig
{
    /// <summary>
    /// Add the <see cref="IAnswerProvider"/> to the given <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The given <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAnswerProviders(this IServiceCollection services)
    {
        services.AddTransient(CreateAnswerProvider);

        return services;
    }

    private static IAnswerProvider CreateAnswerProvider(IServiceProvider serviceProvider)
    {
        var configuration = serviceProvider.GetRequiredService<Configuration>();
        return configuration.ConnectionString?.Provider switch
        {
            null => throw new InvalidOperationException("No connection string specified."),
            "OpenAI" => new SimpleOpenAiAnswerProvider(configuration.ConnectionString),
            _ => throw new InvalidOperationException($"Unsupported provider {configuration.ConnectionString.Provider}"),
        };
    }
}
