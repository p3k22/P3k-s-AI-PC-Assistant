namespace OpenAITextToSpeech.Services;

using Microsoft.Extensions.Logging;

using OpenAITextToSpeech.Models;

using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class TextToSpeech(IHttpClientFactory httpClientFactory, ILogger<TextToSpeech> logger)
{
   /// <summary>
   /// Sends a HttpRequest to OpenAIs TTS service
   /// </summary>
   /// <param name="prompt">Prompt containing input text to be converted to speech</param>
   /// <param name="cancellationToken">For premature cancellation</param>
   /// <returns>An MP3 byte array for processing as a stream for audio playback</returns>
   public async Task<byte[]?> GetResponseAsync(Prompt? prompt, CancellationToken cancellationToken = default)
   {
      try
      {
         // Prompt validation
         if (prompt == null || string.IsNullOrEmpty(prompt.Input))
         {
            logger.LogError("Prompt cannot be empty.");
            return null;
         }

         // Send the request to Google TTS API
         logger.LogDebug("Sending Prompt to OpenAI-TTS");
         var httpClient = httpClientFactory.CreateClient();
         var requestMessage = GetRequestMessage(prompt);
         var response = await httpClient.SendAsync(
                        requestMessage,
                        HttpCompletionOption.ResponseContentRead,
                        cancellationToken);

         if (!response.IsSuccessStatusCode)
         {
            logger.LogError("API Response error: {message}", response.StatusCode);
            return null;
         }

         // Read the response and return as byte[]
         logger.LogDebug("Returning response as byte array to controller");
         return await response.Content.ReadAsByteArrayAsync(cancellationToken);
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

   /// <summary>
   /// Prepares the HttpRequestMessage format
   /// </summary>
   /// <param name="prompt"></param>
   /// <returns>A correctly formatted and structured HttpRequestMessage</returns>
   private static HttpRequestMessage GetRequestMessage(Prompt prompt)
   {
      var uri = "https://api.openai.com/v1/audio/speech";
      var content = new StringContent(
      JsonSerializer.Serialize(
      new
         {
            model = prompt.Model,
            input = prompt.Input,
            voice = prompt.Voice,
            response_format = prompt.ResponseFormat,
            speed = prompt.Speed
         }),
      Encoding.UTF8,
      "application/json");
      var message = new HttpRequestMessage(HttpMethod.Post, uri);
      message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIApiKey.GetKey);
      message.Content = content;
      return message;
   }
}