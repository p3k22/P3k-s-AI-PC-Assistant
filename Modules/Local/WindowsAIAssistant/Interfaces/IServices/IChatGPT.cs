namespace WindowsAIAssistant.Interfaces.IServices;

using ChatGPT.Interfaces;

/// <summary>
///    Send a prompt to ChatGPT and stream the response
/// </summary>
public interface IChatGPT
{
   List<IMessage> Messages { get; }

   /// <summary>
   ///    Send a list of string where the last index will be used as prompt while previous index's act as context for the gpt.
   ///    Response is streamed back in chunks
   /// </summary>
   /// <param name="prompt">List of messages to send to ChatGPT</param>
   /// <param name="model">Default = gpt-4o-mini</param>
   /// <param name="maxTokens">Default = 700</param>
   /// <returns></returns>
   Task SendMessageAsync(string message, CancellationToken cancellationToken = default);
}