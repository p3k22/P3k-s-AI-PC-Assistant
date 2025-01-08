namespace ChatGPT.Services;

using ChatGPT.Interfaces;
using ChatGPT.Models;

using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

public class MessageRelay(IHttpClientFactory httpClientFactory) : IMessageRelay
{
   public async IAsyncEnumerable<string> StreamMessageResponseAsync(
      List<IMessage> messages,
      int modelIndex = 0,
      int maxTokens = 700,
      [EnumeratorCancellation] CancellationToken cancellationToken = default)
   {
      var serverUrl = "";
      var model = GptLlm.GetModelByIndex(modelIndex);
      var requestBody = new {Model = model, Messages = messages, MaxTokens = maxTokens};
      var jsonString = JsonSerializer.Serialize(requestBody);
      var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverUrl) {Content = requestContent};

      // Send HTTP request and yield an error if the request fails
      var response = await SendRequestAsync(requestMessage, cancellationToken);
      if (response == null)
      {
         yield break;
      }

      // If successful, stream the response content line-by-line
      await foreach (var line in ReadResponseAsync(response, cancellationToken))
      {
         yield return line;
      }
   }

   private static async IAsyncEnumerable<string> ReadResponseAsync(
      HttpResponseMessage response,
      [EnumeratorCancellation] CancellationToken cancellationToken)
   {
      await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
      using var reader = new StreamReader(stream);

      while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
      {
         var line = await reader.ReadLineAsync(cancellationToken);
         if (!string.IsNullOrWhiteSpace(line))
         {
            yield return line!;
         }
      }
   }

   private async Task<HttpResponseMessage?> SendRequestAsync(
      HttpRequestMessage requestMessage,
      CancellationToken cancellationToken)
   {
      try
      {
         var httpClient = httpClientFactory.CreateClient();
         var response = await httpClient.SendAsync(
                        requestMessage,
                        HttpCompletionOption.ResponseHeadersRead,
                        cancellationToken);
         return response.IsSuccessStatusCode ? response : null;
      }
      catch (TaskCanceledException)
      {
         return null;
      }
      catch (Exception)
      {
         return null;
      }
   }
}