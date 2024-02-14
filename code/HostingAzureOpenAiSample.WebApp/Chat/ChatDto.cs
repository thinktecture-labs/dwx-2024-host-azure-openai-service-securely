using System.Collections.Generic;

namespace HostingAzureOpenAiSample.WebApp.Chat;

public sealed class ChatDto
{
    private List<ChatMessageDto>? _messages;

    public List<ChatMessageDto> Messages
    {
        get => _messages ??= [];
        set => _messages = value;
    }
}