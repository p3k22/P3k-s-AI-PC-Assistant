namespace TextToSpeech.Models;

using MyLogger;

using NAudio.Wave;

using System.Collections.Concurrent;

using TextToSpeech.Interfaces;

public class AudioStreamPlayer(Logger? logger = null) : IAudioStreamPlayer
{
   private CancellationToken _cancellationToken;

   private ConcurrentQueue<Stream> _audioQueue = [];

   private Task? _playbackTask;

   public void EnqueueStream(Stream stream, CancellationToken cancellationToken = default)
   {
      if (stream == Stream.Null)
      {
         logger?.WriteDebug("Cannot enqueue null stream");
         return;
      }

      logger?.WriteDebug("Enqueueing audio stream");
      _cancellationToken = cancellationToken;
      _audioQueue.Enqueue(stream);
      StartDequeuing();
   }

   public void StartDequeuing()
   {
      if (_playbackTask == null || _playbackTask.IsCompleted)
      {
         _playbackTask = Task.Run(PlayAudioQueueAsync, _cancellationToken);
      }
   }

   /// <summary>
   ///    Plays a single audio stream.
   /// </summary>
   /// <param name="stream">The stream to play.</param>
   private async Task PlayStreamAsync(Stream stream)
   {
      // Use NAudio's WaveOutEvent and Mp3FileReader to play the MP3 stream
      using (var waveOut = new WaveOutEvent())
      await using (var mp3Reader = new Mp3FileReader(stream)) // Reading MP3 from the buffered stream
      {
         waveOut.Init(mp3Reader);
         waveOut.Play();

         // Wait for the playback to complete
         while (waveOut.PlaybackState == PlaybackState.Playing)
         {
            await Task.Delay(100, _cancellationToken); // Poll every 100ms to check if it's still playing
         }
      }

      // Dispose of the stream if it's no longer needed
      await stream.DisposeAsync();
   }

   /// <summary>
   ///    Plays the enqueued audio streams.
   /// </summary>
   private async Task PlayAudioQueueAsync()
   {
      while (!_cancellationToken.IsCancellationRequested)
      {
         if (!_audioQueue.IsEmpty)
         {
            while (_audioQueue.TryDequeue(out var stream))
            {
               if (_cancellationToken.IsCancellationRequested)
               {
                  logger?.WriteDebug($"Audio Player was cancelled");
                  break;
               }

               logger?.WriteDebug("Playing next stream");
               try
               {
                  await PlayStreamAsync(stream);
               }
               catch (Exception e)
               {
                  logger?.WriteDebug($"Exception caught so cancelling audio player");
               }

               if (_audioQueue.IsEmpty)
               {
                  logger?.WriteDebug($"Audio Queue is now empty. No further playback");
                  break;
               }
            }
         }
         else
         {
            logger?.WriteDebug("Audio Queue is empty. Starting Delay Timer.");
            var totalDelay = 0;
            while (totalDelay < 2000)
            {
               await Task.Delay(100, _cancellationToken); // Wait for 100 ms
               totalDelay += 100;

               if (!_audioQueue.IsEmpty)
               {
                  logger?.WriteDebug($"New audio streams enqueued after {totalDelay}ms. Continuing processing.");
                  break; // Exit the 2000ms delay and process the new text
               }
            }

            // If after 2000 ms, the queue is still empty, end the task
            if (_audioQueue.IsEmpty && totalDelay >= 2000)
            {
               logger?.WriteDebug("No more audio streams after 2000ms. Ending task.");
               break;
            }
         }
      }

      _audioQueue = [];
      _playbackTask = null;
   }
}