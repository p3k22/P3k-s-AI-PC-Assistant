namespace GoogleTextToSpeech.Models;

using System.Text.Json.Serialization;

public class Prompt
{
   /// <summary>
   /// Required: The input text to generate audio for (up to 4096 characters).
   /// </summary>
   [JsonPropertyName("text")]
   public string Text { get; set; } = string.Empty;

   /// <summary>
   /// Required: The language code of the voice to use for speech synthesis (e.g., "en-US").
   /// </summary>
   [JsonPropertyName("languageCode")]
   public string LanguageCode { get; set; } = "en-gb";

   /// <summary>
   /// Required: The name of the voice to use when generating audio (e.g., "en-US-Standard-F").
   /// </summary>
   [JsonPropertyName("name")]
   public string Name { get; set; } = "en-gb-standard-a";

   /// <summary>
   /// Required: Specifies the audio encoding of the output audio stream.
   /// </summary>
   [JsonPropertyName("audioEncoding")]
   public string AudioEncoding { get; set; } = "MP3";

   /// <summary>
   /// Optional: The speaking rate of the generated audio. Defaults to 1.0 (normal speed). Range: 0.25 to 4.0.
   /// </summary>
   [JsonPropertyName("speakingRate")]
   public float SpeakingRate { get; set; } = 1.0f;

   /// <summary>
   /// Optional: The pitch of the generated audio. Defaults to 0 (normal pitch). Range: -20.0 to 20.0.
   /// </summary>
   [JsonPropertyName("pitch")]
   public float Pitch { get; set; } = 0.0f;

   /// <summary>
   /// Optional: The volume gain of the generated audio. Defaults to 0 dB. Range: -96.0 dB to 16.0 dB.
   /// </summary>
   [JsonPropertyName("volumeGainDb")]
   public float VolumeGainDb { get; set; } = 0.0f;
}
