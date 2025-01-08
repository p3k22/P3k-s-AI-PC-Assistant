namespace ServerHost.Controllers;

using ChatGPT.Models;
using ChatGPT.Services;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ai-assistant/[controller]")]
public class ChatController(ChatGptApi chatGptApi, ILogger<ChatController> logger) : ControllerBase
{
   [HttpPost]
   public async Task SendPromptAsync([FromBody] Prompt prompt, CancellationToken cancellationToken)
   {
      try
      {
         // Send prompt and receive the stream from the external service (ChatGPTAssistant)
         logger.LogDebug("Sending prompt to ChatGPTAssistant.");
         await using var gptStream = await chatGptApi.PromptRelayAsync(prompt, cancellationToken);

         // Stream the data from ChatGPTAssistant directly to the client's Response.Body
         logger.LogDebug("Streaming response from ChatGPTAssistant.");
         Response.ContentType = "text/event-stream";
         await gptStream!.CopyToAsync(Response.Body, cancellationToken);
         await Response.Body.FlushAsync(cancellationToken); // Ensure data is immediately sent to the client
      }
      catch (OperationCanceledException)
      {
         logger.LogWarning("The operation was canceled.");
         Response.StatusCode = StatusCodes.Status205ResetContent;
         await Response.WriteAsync("The operation was canceled.", cancellationToken);
      }
      catch (Exception ex)
      {
         logger.LogError(ex, "Error streaming from ChatGPT API.");
         Response.StatusCode = StatusCodes.Status500InternalServerError;
         await Response.WriteAsync($"Error streaming from GPT API: {ex.Message}", cancellationToken);
      }
   }
}

// curl -X POST myserver-hiddenforGitai-assistant/chat -H "Content-Type: application/json" -d "{\"model\": \"gpt-3.5-turbo\", \"messages\": [{\"role\": \"user\", \"content\": \"Hello\"}], \"maxTokens\": 100}" 