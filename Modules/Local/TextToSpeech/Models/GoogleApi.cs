namespace TextToSpeech.Models;

using MyLogger;

using System.Text;
using System.Text.Json;

using TextToSpeech.Helpers;
using TextToSpeech.Interfaces;

public class GoogleApi(IHttpClientFactory httpClientFactory, Logger? logger = null) : ITextToSpeechApi
{
   public string ApiName => "Google";

   public string Model => "en-GB";

   public string ResponseFormat => "MP3";

   public float Speed => 1.0f;

   public string Voice => "en-GB-Standard-A";

   public async Task<string?> ConvertToSpeech(string text, CancellationToken cancellationToken = default)
   {
      var serverUrl = "";
      var requestBody = new
                           {
                              Text = text,
                              LanguageCode = Model,
                              Name = Voice,
                              AudioEncoding = ResponseFormat,
                              SpeakingRate = Speed
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