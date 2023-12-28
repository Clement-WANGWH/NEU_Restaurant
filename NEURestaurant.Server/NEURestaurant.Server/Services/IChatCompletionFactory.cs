using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI.ChatCompletion;

namespace DailyPoetryH.Server.Services;

public interface IChatCompletionFactory {
    IChatCompletion GetChatCompletion();
}