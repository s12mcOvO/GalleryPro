using Microsoft.Extensions.AI;
using OpenAI;
using System.ClientModel;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using GalleryPro;


string path = @"./";
var imageFiles = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
    .Where(f => f.EndsWith(".png") || f.EndsWith(".jpg") || f.EndsWith(".jpeg") || f.EndsWith(".gif"))
    .ToArray();

foreach (var imagePath in imageFiles)
{
    Console.WriteLine($"Process: {Path.GetFileName(imagePath)}");
    try
    {
        var imageBytes = File.ReadAllBytes(imagePath);
        var label = await ChatService.AiChatWithImageAsync("What's the picture about? Please give it a label that you think closest. Answer me only use one word", imageBytes);
        Console.WriteLine($"Image: {Path.GetFileName(imagePath)} -> Label: {label}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error processing {imagePath}: {ex.Message}");
    }
}
