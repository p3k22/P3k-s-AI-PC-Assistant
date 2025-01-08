namespace WindowsAIAssistant.Interfaces;

/// <summary>
///    $SUMMARY$
/// </summary>
public interface ITextToSpeech
{
   void ConvertTextToSpeech(string text, CancellationToken cancellationToken);

   Task StopProcessAsync();
}