using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HostingAzureOpenAiSample.WebApp.OpenAiAccess;
using Light.GuardClauses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using OpenAI;
using OpenAI.Chat;
using Serilog;

namespace HostingAzureOpenAiSample.WebApp.Chat;

public static class ChatEndpoint
{
    public static WebApplication MapOpenAiChat(this WebApplication app)
    {
        app.MapPost("/api/chat", StreamChatResponse);
        return app;
    }

    private static async Task StreamChatResponse(
        OpenAIClient openAiClient,
        OpenAiAccessOptions openAiAccessOptions,
        HttpResponse response,
        ChatDto dto,
        ILogger logger,
        CancellationToken cancellationToken
    )
    {
        response.StatusCode = StatusCodes.Status200OK;
        response.ContentType = "text/plain";
        var messages = new List<ChatMessage>(dto.Messages.Count + 1)
        {
            new SystemChatMessage(
                "You are a helpful assistant. Please keep your answers short and to the point."
            )
        };
        messages.AddRange(
            dto.Messages.Select(
                m => m.Originator == Originators.Ai ?
                    (ChatMessage) new AssistantChatMessage(m.Text) :
                    new UserChatMessage(m.Text)
            )
        );

        var chatClient = openAiClient.GetChatClient(openAiAccessOptions.ModelName);
        var streamingResponse = chatClient.CompleteChatStreamingAsync(messages, cancellationToken: cancellationToken);
        await foreach (var item in streamingResponse)
        {
            logger.Debug("{@StreamItem}", item);
            foreach (var contentPart in item.ContentUpdate)
            {
                if (contentPart.Kind == ChatMessageContentPartKind.Text && !contentPart.Text.IsNullOrEmpty())
                {
                    await response.WriteAsync(contentPart.Text, cancellationToken);
                }
            }
        }
    }
}