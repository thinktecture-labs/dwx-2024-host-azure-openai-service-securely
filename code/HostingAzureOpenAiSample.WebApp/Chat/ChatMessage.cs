using System;

namespace HostingAzureOpenAiSample.WebApp.Chat;

public sealed class ChatMessageDto
{
    public Guid Id { get; set; }
    public string Originator { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}