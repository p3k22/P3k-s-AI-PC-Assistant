namespace ServerHost.Controllers;

using GoogleTextToSpeech.Models;
using GoogleTextToSpeech.Services;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ai-assistant/[controller]")]
public class GoogleTtsController(TextToSpeech textToSpeech, ILogger<GoogleTtsController> logger) : ControllerBase
{
   [HttpPost]
   public async Task<IActionResult> SendPrompt([FromBody] Prompt prompt, CancellationToken cancellationToken)
   {
      try
      {
         // Send prompt and receive the string response from the TTS service
         logger.LogDebug("Forwarding prompt to Google TTS Module.");
         var speechResult = await textToSpeech.GetResponseAsync(prompt, cancellationToken);

         // Return the string response
         logger.LogDebug("Returning response as base64 encoded string to client.");
         return Ok(speechResult); // Ok() returns a 200 response with the string content
      }
      catch (OperationCanceledException)
      {
         logger.LogWarning("The operation was canceled.");
         return StatusCode(StatusCodes.Status205ResetContent, "The operation was canceled.");
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "Error streaming from TTS API.");
         return StatusCode(StatusCodes.Status500InternalServerError, $"Error streaming from TTS API: {ex.Message}");
      }
   }
}

// curl -X POST "myserver-hiddenforGitai-assistant/googletts" -H "Content-Type: application/json" -d "{\"text\": \"Hello, how can I help you today?\", \"languageCode\": \"en-GB\", \"name\": \"en-GB-Standard-A\", \"audioEncoding\": \"MP3\", \"speakingRate\": 1.0, \"pitch\": 0, \"volumeGainDb\": 0}"