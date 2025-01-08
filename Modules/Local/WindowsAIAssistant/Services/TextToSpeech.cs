namespace WindowsAIAssistant.Services;

using global::TextToSpeech.Interfaces;

using MyLogger;

using WindowsAIAssistant.Interfaces;

public class TextToSpeech(ITextToSpeechProcessor tts, Logger? logger = null) : ITextToSpeech
{
   private CancellationTokenSource? _cancellationTokenSource;

   public void A_TestFunction()
   {
      logger?.WriteDebug("Test Func Init");
      _ = A_TestFunctionB();
   }

   public void ConvertTextToSpeech(string text, CancellationToken cancellationToken)
   {
      _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      tts.EnqueueText(text, _cancellationTokenSource.Token);
   }

   public async Task StopProcessAsync()
   {
      logger?.WriteDebug("Stopping current Text To Speech process");
      await _cancellationTokenSource?.CancelAsync()!;
   }

   /// <summary>
   /// Test script for testing timed interruptions via cancellation token
   /// </summary>
   /// <returns></returns>
   private async Task A_TestFunctionB()
   {
      //var cSource = new CancellationTokenSource();
      //var texts = new[] {"Hello World its nice to meet you!", "It sure has been a fun night.", "Ted"};
      //for (var i = 0; i < texts.Length; i++)
      //{
      //   ConvertTextToSpeech(texts[i], cSource.Token);
      //}

      //await Task.Delay(3300);
      //logger?.WriteDebug(" times cancelling");
      //await cSource.CancelAsync();
      //await StopProcessAsync();

      //await Task.Delay(400);
      //for (var i = 0; i < texts.Length; i++)
      //{
      //   ConvertTextToSpeech(texts[i], cSource.Token);
      //}

      //await Task.Delay(1200);
      //await cSource.CancelAsync();
   }
}