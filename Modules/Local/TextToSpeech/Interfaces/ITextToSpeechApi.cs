namespace TextToSpeech.Interfaces;

/// <summary>
///    Interface for a Text-to-Speech service.
/// </summary>
public interface ITextToSpeechApi
{
   /// <summary>
   /// Name of API for references
   /// </summary>
   string ApiName { get; }

   /// <summary>
   ///    TTS model to use.
   /// </summary>
   string Model { get; }

   /// <summary>
   ///    Audio format of response.
   /// </summary>
   string ResponseFormat { get; }

   /// <summary>
   ///    Playback speed.
   /// </summary>
   float Speed { get; }

   /// <summary>
   ///    TTS voice to use.
   /// </summary>
   string Voice { get; }

   /// <summary>
   ///    Asynchronously sends text to server for audio conversion.
   /// </summary>
   /// <param name="text">The text to be converted into speech.</param>
   /// <param name="cancellationToken">
   ///    Optional. A token to monitor for cancellation requests, allowing the operation to be
   ///    cancelled prematurely.
   /// </param>
   /// <returns>A base64 encoded string to be converted to an audio stream.</returns>
   Task<string?> ConvertToSpeech(string text, CancellationToken cancellationToken = default);
}