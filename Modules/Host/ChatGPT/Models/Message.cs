namespace ChatGPT.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Hold user prompts and AI responses.
/// </summary>
public class Message
{
   /// <summary>
   ///    The content of the message. This is the actual text of the message.
   /// </summary>
   [JsonPropertyName("content")]
   public string Content { get; set; } = string.Empty;

   /// <summary>
   ///    The role of the message sender. Can be "system", "user", or "assistant".
   /// </summary>
   [JsonPropertyName("role")]
   public string Role { get; set; } = string.Empty;
}