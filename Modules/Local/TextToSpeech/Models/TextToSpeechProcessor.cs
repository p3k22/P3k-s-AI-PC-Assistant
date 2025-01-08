namespace TextToSpeech.Models;

using MyLogger;

using System.Collections.Concurrent;
using System.Text.RegularExpressions;

using TextToSpeech.Interfaces;

/// <summary>
///    Manages text-to-speech processing, using a specified TTS API to convert text to speech and an audio player to play
///    the resulting audio.
/// </summary>
public class TextToSpeechProcessor(
   ITextToSpeechApiManager textToSpeechApi,
   IAudioStreamPlayer audioStreamPlayer,
   Logger? logger = null) : ITextToSpeechProcessor
{
   private CancellationToken _cancellationToken;

   private ConcurrentQueue<string> _textQueue = [];

   private Task? _convertTtsTask;

   /// <summary>
   ///    Asynchronously enqueues text to be processed into speech.
   /// </summary>
   /// <param name="text">The text to process.</param>
   public void EnqueueText(string text, CancellationToken cancellationToken = default)
   {
      _cancellationToken = cancellationToken;
      _textQueue.Enqueue(text);
      StartDequeuing();
   }

   private MemoryStream DecodedResponse(string? base64Response)
   {
      if (string.IsNullOrEmpty(base64Response))
      {
         logger?.WriteDebug("Empty or null response received.");
         return null!;
      }

      // Decode the Base64 string into a byte array
      var cleanedBase64 = Regex.Replace(base64Response, @"[^A-Za-z0-9+/=]", "");
      var decodedBytes = Convert.FromBase64String(cleanedBase64);
      return new MemoryStream(decodedBytes);
   }

   private async Task DequeueTextQueueAsync()
   {
      while (!_cancellationToken.IsCancellationRequested)
      {
         if (!_textQueue.IsEmpty)
         {
            // Dequeue and process all items in the queue
            while (_textQueue.TryDequeue(out var text))
            {
               logger?.WriteDebug($"Converting... {text}");
               var encodedResponse = await textToSpeechApi.CurrentApi.ConvertToSpeech(text, _cancellationToken);
               audioStreamPlayer.EnqueueStream(DecodedResponse(encodedResponse), _cancellationToken);
            }
         }
         else
         {
            // Start a 2000 ms wait period, checking every 100 ms if new text arrives
            logger?.WriteDebug("Text Queue is empty. Starting 2000ms Delay Timer.");
            var totalDelay = 0;
            while (totalDelay < 2000)
            {
               await Task.Delay(100, _cancellationToken); // Wait for 100 ms
               totalDelay += 100;

               if (!_textQueue.IsEmpty)
               {
                  logger?.WriteDebug($"New text enqueued after {totalDelay}ms. Continuing processing.");
                  break; // Exit the 2000ms delay and process the new text
               }
            }

            // If after 2000 ms, the queue is still empty, end the task
            if (_textQueue.IsEmpty && totalDelay >= 2000)
            {
               logger?.WriteDebug("No more text after 2000ms. Ending task.");
               break;
            }
         }
      }

      _textQueue = [];
      _convertTtsTask = null; // Mark the task as completed
   }

   private void StartDequeuing()
   {
      if (_convertTtsTask == null || _convertTtsTask.IsCompleted)
      {
         _convertTtsTask = Task.Run(DequeueTextQueueAsync, _cancellationToken);
      }
   }
}