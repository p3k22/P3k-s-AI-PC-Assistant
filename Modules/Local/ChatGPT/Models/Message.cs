namespace ChatGPT.Models;

using ChatGPT.Interfaces;

public class Message(string role, string content) : IMessage
{
   public string Content { get; set; } = content;

   public string Role { get; set; } = role;
}