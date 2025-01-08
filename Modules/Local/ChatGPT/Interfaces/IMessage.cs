namespace ChatGPT.Interfaces;

/// <summary>
///    Message class for sending prompts to ChatGPT
/// </summary>
public interface IMessage
{
   /// <summary>
   ///    The content of the message. This is the actual text of the message.
   /// </summary>
   string Content { get; set; }

   /// <summary>
   ///    The role of the message sender. Can be "system", "user", or "assistant".
   /// </summary>
   string Role { get; set; }
}