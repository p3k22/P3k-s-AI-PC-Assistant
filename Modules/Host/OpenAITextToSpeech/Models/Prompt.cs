namespace OpenAITextToSpeech.Models;

using System.Text.Json.Serialization;

public class Prompt
{
   /// <summary>
   ///    Required: The input text to generate audio for (up to 4096 characters).
   /// </summary>
   [JsonPropertyName("input")]
   public string Input { get; set; } = string.Empty;

   /// <summary>
   ///    Required: The model to use, either "tts-1" or "tts-1-hd".
   /// </summary>
   [JsonPropertyName("model")]
   public string Model { get; set; } = "tts-1";

   /// <summary>
   ///    Required: The voice to use when generating audio (e.g., "alloy", "echo", "fable", "onyx", "nova", "shimmer").
   /// </summary>
   [JsonPropertyName("voice")]
   public string Voice { get; set; } = "alloy";

   /// <summary>
   ///    Optional: The format of the audio output. Defaults to "mp3".
   /// </summary>
   [JsonPropertyName("response_format")]
   public string ResponseFormat { get; set; } = "mp3";

   /// <summary>
   ///    Optional: The speed of the generated audio. Defaults to 1.0 (normal speed). Range: 0.25 to 4.0.
   /// </summary>
   [JsonPropertyName("speed")]
   public float Speed { get; set; } = 1.0f;
}