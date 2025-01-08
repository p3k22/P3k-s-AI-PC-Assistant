namespace ChatGPT.Services;

using ChatGPT.Models;

using Microsoft.Extensions.Logging;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

/// <summary>
///    Class library for interacting with Chat-GPT
/// </summary>
/// <param name="httpClientFactory">DI httpClient</param>
public class ChatGptApi(IHttpClientFactory httpClientFactory, ILogger<ChatGptApi> logger)
{
   /// <summary>
   ///    Method for sending text prompt to ChatGPT token service and streaming back response in real time.
   /// </summary>
   /// <param name="prompt"></param>
   /// <param name="cancellationToken"></param>
   /// <returns></returns>
   public async Task<Stream?> PromptRelayAsync(Prompt? prompt, CancellationToken cancellationToken = default)
   {
      try
      {
         // Prompt validation
         if (prompt == null || string.IsNullOrEmpty(prompt.Messages[^1].Content))
         {
            logger.LogError("Prompt cannot be empty.");
            return null;
         }

         // Send the request to the GPT API
         logger.LogDebug("Sending Prompt to Chat-GPT");
         var httpClient = httpClientFactory.CreateClient();
         var requestMessage = RequestMessageSetup(prompt);
         var responseStream = await httpClient.SendAsync(
                              requestMessage,
                              HttpCompletionOption.ResponseHeadersRead,
                              cancellationToken);

         // Handle non-success status codes
         if (!responseStream.IsSuccessStatusCode)
         {
            logger.LogError("Error from GPT API: {response}", responseStream.ReasonPhrase);
            return null;
         }

         // Return the stream to the Server Host
         logger.LogDebug("Reading Response as Stream");
         return await responseStream.Content.ReadAsStreamAsync(cancellationToken);
      }
      catch (OperationCanceledException)
      {
         // Handle task cancellation gracefully
         logger.LogWarning("The operation was canceled.");
         return null;
      }
      catch (HttpRequestException httpEx)
      {
         // Handle HTTP-specific errors
         logger.LogError("HTTP error occurred: {message}", httpEx.Message);
         return null;
      }
      catch (Exception e)
      {
         // Handle any other exceptions
         logger.LogError("An unexpected error occurred: {message}", e.Message);
         return null;
      }
   }

   private static HttpRequestMessage RequestMessageSetup(Prompt prompt)
   {
      var uri = "https://api.openai.com/v1/chat/completions";
      var content = new StringContent(
      JsonSerializer.Serialize(
      new
         {
            model = prompt.Model,
            messages = prompt.Messages,
            max_tokens = prompt.MaxTokens,
            stream = true,
            stream_options = new StreamOptions {IncludeUsage = true}
         }),
      Encoding.UTF8,
      "application/json");
      var message = new HttpRequestMessage(HttpMethod.Post, uri);
      message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIApiKey.GetKey);
      message.Content = content;
      return message;
   }
}