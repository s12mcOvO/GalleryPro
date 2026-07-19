using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

namespace GalleryPro;

/// <summary>
/// Provides a simple wrapper around the OpenAI chat client.
/// </summary>
public static class ChatService
{
    // Reuse the same client configuration as in Program.cs
    private static readonly IChatClient _chatClient;

    // Static constructor to initialize the client once
    static ChatService()
    {
        var localLmStudioEndpoint = new Uri("http://localhost:1234/v1");
        string dummyApiKey = "lm-studio";
        string yourModelId = "qwen/qwen2.5-vl-7b";

        var openAIClient = new OpenAIClient(
            new ApiKeyCredential(dummyApiKey),
            new OpenAIClientOptions
            {
                Endpoint = localLmStudioEndpoint
            });

        _chatClient = openAIClient
            .GetChatClient(yourModelId)
            .AsIChatClient();
    }

    /// <summary>
    /// Sends a text question to the model and returns its response.
    /// </summary>
    public static async Task<string> AiChatAsync(string question)
    {
        var userMessage = new ChatMessage
        {
            Role = ChatRole.User,
            Contents = new List<AIContent>
            {
                new TextContent(question)
            }
        };

        var response = await _chatClient.GetResponseAsync(new[] { userMessage });
        return response.Text;
    }

    /// <summary>
    /// Sends a question with an image to the model and returns its response.
    /// </summary>
    public static async Task<string> AiChatWithImageAsync(string question, byte[] imageBytes)
    {
        var userMessage = new ChatMessage
        {
            Role = ChatRole.User,
            Contents = new List<AIContent>
            {
                new TextContent(question),
                new DataContent(imageBytes, "image/png")
            }
        };

        var response = await _chatClient.GetResponseAsync(new[] { userMessage });
        return response.Text;
    }
}
