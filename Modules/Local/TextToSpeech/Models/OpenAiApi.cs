namespace TextToSpeech.Models;

using MyLogger;

using System.Text;
using System.Text.Json;

using TextToSpeech.Helpers;
using TextToSpeech.Interfaces;

public class OpenAiApi(IHttpClientFactory httpClientFactory, Logger? logger = null) : ITextToSpeechApi
{
   public string ApiName => "OpenAI";

   public string Model => "tts-1";

   public string ResponseFormat => "mp3";

   public float Speed => 1.0f;

   public string Voice => "alloy";

   public async Task<string?> ConvertToSpeech(string text, CancellationToken cancellationToken = default)
   {
      var serverUrl = "";
      var requestBody = new
                           {
                              Input = text,
                              Model,
                              Voice,
                              ResponseFormat,
                              Speed
                           };
      var jsonString = JsonSerializer.Serialize(requestBody);
      var requestContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, serverUrl) {Content = requestContent};

      logger?.WriteDebug("Sending prompt");
      var response = await HttpRequestHelper.SendRequestAsync(
                     httpClientFactory.CreateClient(),
                     requestMessage,
                     cancellationToken);
      if (response == null)
      {
         logger?.WriteDebug("Response is null");
         return null;
      }

      logger?.WriteDebug("Reading response");
      return await response.Content.ReadAsStringAsync(cancellationToken);
   }
}