namespace TextToSpeech.Interfaces;

/// <summary>
///    Defines the interface for text-to-speech processing capabilities.
/// </summary>
public interface ITextToSpeechProcessor
{
   /// <summary>
   ///    Enqueues text to be converted to speech asynchronously.
   /// </summary>
   /// <param name="text">The text to be processed into speech.</param>
   void EnqueueText(string text, CancellationToken cancellationToken = default);
}