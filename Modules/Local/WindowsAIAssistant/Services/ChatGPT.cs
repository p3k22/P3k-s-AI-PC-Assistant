namespace WindowsAIAssistant.Services;

using global::ChatGPT.Interfaces;
using global::ChatGPT.Models;

using MyLogger;

using WindowsAIAssistant.Interfaces.IServices;

public class ChatGPT(IMessageRelay messageRelay, Logger? logger = null) : IChatGPT
{
   public List<IMessage> Messages { get; private set; } = [];

   public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
   {
      Messages.Add(new Message("user", message));
      await foreach (var line in messageRelay.StreamMessageResponseAsync(
                     Messages,
                     cancellationToken: cancellationToken))
      {
         logger?.WriteDebug(line);
      }
   }
}