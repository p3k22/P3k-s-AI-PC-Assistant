namespace TextToSpeech.Interfaces;

/// <summary>
///    Interface for playing converted audio streams from text.
/// </summary>
public interface IAudioStreamPlayer
{
   /// <summary>
   ///    Enqueues an audio stream for playback.
   /// </summary>
   /// <param name="stream">The audio stream to enqueue.</param>
   void EnqueueStream(Stream stream, CancellationToken cancellationToken = default);

   /// <summary>
   ///    Starts the playback of enqueued audio streams.
   /// </summary>
   void StartDequeuing();
}