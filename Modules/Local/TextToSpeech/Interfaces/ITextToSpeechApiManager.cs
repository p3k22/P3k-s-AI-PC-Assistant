namespace TextToSpeech.Interfaces;

/// <summary>
///    Controls which TextToSpeech API is currently in use
/// </summary>
public interface ITextToSpeechApiManager
{
   /// <summary>
   ///    Add available APIs for usage
   /// </summary>
   Dictionary<string, ITextToSpeechApi> Apis { get; }

   /// <summary>
   ///    Current API to use
   /// </summary>
   ITextToSpeechApi CurrentApi { get; }

   /// <summary>
   ///    Add an TTS API to the API dictionary
   /// </summary>
   /// <param name="api"></param>
   void AddApi(ITextToSpeechApi api);

   /// <summary>
   ///    Change the current API by name
   /// </summary>
   /// <param name="name">The API name to use as current.</param>
   void ChangeApi(string name);
}