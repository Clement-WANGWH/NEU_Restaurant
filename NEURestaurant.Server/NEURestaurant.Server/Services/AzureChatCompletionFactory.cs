using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;

namespace DailyPoetryH.Server.Services;

public class AzureChatCompletionFactory : IChatCompletionFactory {
    public const string ModelId =
        nameof(AzureChatCompletionFactory) + "." + nameof(ModelId);

    public const string Endpoint =
        nameof(AzureChatCompletionFactory) + "." + nameof(Endpoint);

    public const string ApiKey =
        nameof(AzureChatCompletionFactory) + "." + nameof(ApiKey);

    private IConfiguration _configuration;

    public AzureChatCompletionFactory(IConfiguration configuration) {
        _configuration = configuration;
    }
    public IChatCompletion GetChatCompletion() =>
        new AzureChatCompletion(_configuration[ModelId],
            _configuration[Endpoint], _configuration[ApiKey]);
}