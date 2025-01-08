namespace TextToSpeech.Models;

using MyLogger;

using TextToSpeech.Interfaces;

/// <summary>
///    Add and assign Text to Speech APIs
/// </summary>
public class ApiManager : ITextToSpeechApiManager
{
   private string _currentApiName = string.Empty;

   private readonly Logger? _logger;

   public ApiManager(IHttpClientFactory httpClientFactory, Logger? logger = null)
   {
      _logger = logger;
      Apis = [];

      AddApi(new GoogleApi(httpClientFactory, logger));
      AddApi(new OpenAiApi(httpClientFactory, logger));

      ChangeApi("google");
   }

   public Dictionary<string, ITextToSpeechApi> Apis { get; }

   public ITextToSpeechApi CurrentApi { get; private set; } = null!;

   public void AddApi(ITextToSpeechApi api)
   {
      Apis.Add(api.ApiName.ToLower(), api);
   }

   public void ChangeApi(string apiName)
   {
      if (Apis.TryGetValue(apiName.ToLower(), out var api))
      {
         CurrentApi = api;
         _currentApiName = apiName;
         _logger?.WriteDebug("Current API:" + _currentApiName);
      }
   }
}