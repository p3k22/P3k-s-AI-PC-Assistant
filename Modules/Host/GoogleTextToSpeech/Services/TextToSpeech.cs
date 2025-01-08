namespace GoogleTextToSpeech.Services
{
   using GoogleTextToSpeech.Models;

   using Microsoft.Extensions.Logging;

   using System.Text;
   using System.Text.Json;

   public class TextToSpeech(IHttpClientFactory httpClientFactory, ILogger<TextToSpeech> logger)
   {
      /// <summary>
      /// Sends a HttpRequest to Google's TTS service
      /// </summary>
      /// <param name="prompt">Prompt containing input text to be converted to speech</param>
      /// <param name="cancellationToken">For premature cancellation</param>
      /// <returns>An MP3 byte array for processing as a stream for audio playback</returns>
      public async Task<string?> GetResponseAsync(Prompt? prompt, CancellationToken cancellationToken = default)
      {
         try
         {
            // Validate prompt
            if (prompt == null || string.IsNullOrEmpty(prompt.Text))
            {
               logger.LogError("Prompt cannot be empty.");
               return null;
            }

            // Send the request to Google TTS API
            logger.LogDebug("Sending prompt to Google TTS");
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

            // Read the response and return as string (google by default gives base64 encoded string)
            logger.LogDebug("Returning response as base64 encoded string to controller");
            return await response.Content.ReadAsStringAsync(cancellationToken);
         }
         catch (OperationCanceledException)
         {
            logger.LogWarning("The operation was canceled.");
            return null;
         }
         catch (HttpRequestException httpEx)
         {
            logger.LogError("HTTP error occurred: {message}", httpEx.Message);
            return null;
         }
         catch (Exception ex)
         {
            logger.LogError("An unexpected error occurred: {message}", ex.Message);
            return null;
         }
      }

      /// <summary>
      /// Prepares the HttpRequestMessage for Google TTS
      /// </summary>
      /// <param name="prompt">TTS request prompt</param>
      /// <returns>A formatted HttpRequestMessage</returns>
      private static HttpRequestMessage GetRequestMessage(Prompt prompt)
      {
         var uri = "https://texttospeech.googleapis.com/v1/text:synthesize?key=" + GoogleApiKey.GetKey;
         var content = new StringContent(
         JsonSerializer.Serialize(
         new
            {
               input = new {text = prompt.Text},
               voice = new {languageCode = prompt.LanguageCode, name = prompt.Name},
               audioConfig = new {audioEncoding = prompt.AudioEncoding}
            }),
         Encoding.UTF8,
         "application/json");
         var message = new HttpRequestMessage(HttpMethod.Post, uri) {Content = content};
         return message;
      }
   }
}
