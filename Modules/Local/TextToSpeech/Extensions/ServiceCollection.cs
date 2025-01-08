namespace TextToSpeech.Extensions;

using Microsoft.Extensions.DependencyInjection;

using TextToSpeech.Interfaces;
using TextToSpeech.Models;

public static class ServiceCollection
{
   /// <summary>
   ///    Adds necessary text-to-speech services to the specified IServiceCollection.
   /// </summary>
   /// <param name="services">The IServiceCollection to add services to.</param>
   /// <returns>The IServiceCollection for chaining.</returns>
   public static IServiceCollection AddTextToSpeechServices(this IServiceCollection services)
   {
      services.AddHttpClient(); // Ensure that IHttpClientFactory is available for TTS APIs
      services.AddLogging();
      services.AddScoped<ITextToSpeechApiManager, ApiManager>();
      services.AddScoped<IAudioStreamPlayer, AudioStreamPlayer>();

      // Scoped ensures a new instance per session or request
      services.AddScoped<ITextToSpeechProcessor, TextToSpeechProcessor>();

      return services;
   }
}