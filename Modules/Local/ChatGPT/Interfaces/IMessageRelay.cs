namespace ChatGPT.Interfaces;

/// <summary>
///    Send a prompt to ChatGPT and stream the response
/// </summary>
public interface IMessageRelay
{
   /// <summary>
   ///    Send a list of string where the last index will be used as prompt while previous index's act as context for the gpt.
   ///    Response is streamed back in chunks
   /// </summary>
   /// <param name="messages">List of messages to send to ChatGPT</param>
   /// <param name="model">Default = gpt-4o-mini</param>
   /// <param name="maxTokens">Default = 700</param>
   /// <returns></returns>
   IAsyncEnumerable<string> StreamMessageResponseAsync(
      List<IMessage> messages,
      int modelIndex = 0,
      int maxTokens = 700,
      CancellationToken cancellationToken = default);
}