namespace ChatGPT.Models;

using System.Text.Json.Serialization;

/// <summary>
///    Flags used for reporting back Token usage during streamed responses.
/// </summary>
public class StreamOptions
{
   /// <summary>
   ///    Include the usage tokens in the last streamed chunk
   /// </summary>
   [JsonPropertyName("include_usage")]
   public bool IncludeUsage { get; set; }
}