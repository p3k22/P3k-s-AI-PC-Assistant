namespace ChatGPT.Models;

using System.Text.Json.Serialization;

/// <summary>
/// Class containing properties required to send a chat request to ChatGPT
/// </summary>
public class Prompt
{
   /// <summary>
   ///    A penalty that discourages the model from repeating the same lines verbatim. A value between -2.0 and 2.0.
   ///    Default is 0.0 (no penalty).
   /// </summary>
   [JsonPropertyName("frequency_penalty")]
   public double FrequencyPenalty { get; set; } = 0.0;

   /// <summary>
   ///    Adjusts the likelihood of specific tokens appearing in the output.
   ///    The keys are token IDs and the values are the bias to apply (-100 to 100).
   ///    Default is an empty dictionary (no bias).
   /// </summary>
   [JsonPropertyName("logit_bias")]
   public Dictionary<string, int> LogitBias { get; set; } = new Dictionary<string, int>();

   /// <summary>
   ///    The maximum number of tokens to generate in the response. This includes both input and output tokens.
   ///    Default is 500.
   /// </summary>
   [JsonPropertyName("max_tokens")]
   public int MaxTokens { get; set; } = 500;

   /// <summary>
   ///    A list of messages that make up the conversation so far. Typically, includes "user" and "assistant" roles.
   ///    This field is required and should contain at least one message.
   /// </summary>
   [JsonPropertyName("messages")]
   public List<Message> Messages { get; set; } = [];

   /// <summary>
   ///    The model to use for generating the response, e.g., "gpt-4".
   ///    Default is "gpt-4".
   /// </summary>
   [JsonPropertyName("model")]
   public string Model { get; set; } = "gpt-4o-mini";

   /// <summary>
   ///    The number of responses to generate. Typically set to 1.
   ///    Default is 1.
   /// </summary>
   [JsonPropertyName("n")]
   public int N { get; set; } = 1;

   /// <summary>
   ///    A penalty that discourages the model from discussing new topics. A value between -2.0 and 2.0.
   ///    Default is 0.0 (no penalty).
   /// </summary>
   [JsonPropertyName("presence_penalty")]
   public double PresencePenalty { get; set; } = 0.0;

   /// <summary>
   ///    The stop sequence(s) where the model should stop generating further tokens. Can be a single string or an array of
   ///    strings.
   ///    Default is null (no stop sequence).
   /// </summary>
   [JsonPropertyName("stop")]
   public string Stop { get; set; } = string.Empty;

   /// <summary>
   ///    Whether to stream the response back incrementally.
   ///    Default is false (response is returned all at once).
   /// </summary>
   [JsonPropertyName("stream")]
   public bool Stream { get; set; } = true;

   /// <summary>
   ///    A pain in the ass identifier for requesting usage tokens from streamed responses.
   ///    Default is False
   /// </summary>
   [JsonPropertyName("stream_options")]
   public StreamOptions StreamOptions { get; set; } = new StreamOptions();

   /// <summary>
   ///    Controls the randomness of the response. Values closer to 0 will make the output more deterministic, while values
   ///    closer to 1 will make it more creative.
   ///    Default is 0.7.
   /// </summary>
   [JsonPropertyName("temperature")]
   public double Temperature { get; set; } = 0.7;

   /// <summary>
   ///    Controls the diversity of the output by adjusting the cumulative probability of token selection.
   ///    Default is 0.95.
   /// </summary>
   [JsonPropertyName("top_p")]
   public double TopP { get; set; } = 0.95;

   /// <summary>
   ///    A unique identifier for the user or session. Useful for tracking or session management.
   ///    Default is null (no user ID).
   /// </summary>
   [JsonPropertyName("user")]
   public string User { get; set; } = string.Empty;
}