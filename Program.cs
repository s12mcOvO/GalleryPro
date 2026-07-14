using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;

Uri localLmStudioEndpoint = new Uri("http://localhost:1234/v1");
string dummyApiKey = "lm-studio";
string yourModelId = "qwen/qwen2.5-vl-7b"; 

var openAIClient = new OpenAIClient(
    new ApiKeyCredential(dummyApiKey),
    new OpenAIClientOptions
    {
        Endpoint = localLmStudioEndpoint
    }
);

IChatClient chatClient = openAIClient
    .GetChatClient(yourModelId)
    .AsIChatClient();

string path = @"./";
string[] imageFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
    .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".gif"))
    .ToArray();

foreach (string imagePath in imageFiles)
{
    Console.WriteLine($"Process: {Path.GetFileName(imagePath)}");
    
    try
    {
        var userMessage = new ChatMessage
        {
            Role = ChatRole.User,
            Contents = new List<AIContent>
            {
                new TextContent("What's the picture about? Please give it a label that you think closest. Answer me only use one word"),
                new DataContent(File.ReadAllBytes(imagePath), "image/png")
            }
        };

        var messages = new List<ChatMessage> { userMessage };
        var response = await chatClient.GetResponseAsync(messages);
        
        Console.WriteLine($"Image: {Path.GetFileName(imagePath)} -> Label: {response.Text}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"It's something went wrong on {imagePath} \n Report: {ex.Message}");
    }
}