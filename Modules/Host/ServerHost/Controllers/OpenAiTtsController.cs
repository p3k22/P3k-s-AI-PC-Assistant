namespace ServerHost.Controllers;

using Microsoft.AspNetCore.Mvc;

using OpenAITextToSpeech.Models;
using OpenAITextToSpeech.Services;

[ApiController]
[Route("api/ai-assistant/[controller]")]
public class OpenAiTtsController(TextToSpeech textToSpeech, ILogger<OpenAiTtsController> logger) : ControllerBase
{
   [HttpPost]
   public async Task<IActionResult> SendPrompt([FromBody] Prompt prompt, CancellationToken cancellationToken)
   {
      try
      {
         // Send prompt and receive the string response from the TTS service
         logger.LogDebug("Forwarding prompt to OpenAI TTS Module.");
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

// curl -X POST myserver-hiddenforGitai-assistant/openaitts -H "Content-Type: application/json" -d "{\"model\": \"tts-1\", \"input\": \"Hello, how can I help you today?\", \"voice\": \"alloy\", \"response_format\": \"mp3\", \"speed\": 1.0}"