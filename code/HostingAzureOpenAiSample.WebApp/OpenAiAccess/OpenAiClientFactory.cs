using System;
using Azure;
using Azure.AI.OpenAI;
using OpenAI;

namespace HostingAzureOpenAiSample.WebApp.OpenAiAccess;

public static class OpenAiClientFactory
{
    public static OpenAIClient CreateOpenAiClient(OpenAiAccessOptions options) =>
        options.Type switch
        {
            OpenAiServiceType.OpenAi => new OpenAIClient(options.ApiKey),
            OpenAiServiceType.AzureOpenAi => new AzureOpenAIClient(new Uri(options.AzureOpenAiEndpoint),
                                                                   new AzureKeyCredential(options.ApiKey)),
            _ => throw new ArgumentException($"{options.Type} is unknown", nameof(options))
        };
}