namespace ChatGPT.Extensions
{
   using ChatGPT.Interfaces;
   using ChatGPT.Models;
   using ChatGPT.Services;

   using Microsoft.Extensions.DependencyInjection;

   public static class ServiceCollection
   {
      /// <summary>
      ///    Adds necessary ChatGPT services to the specified IServiceCollection.
      /// </summary>
      /// <param name="services">The IServiceCollection to add services to.</param>
      /// <returns>The IServiceCollection for chaining.</returns>
      public static IServiceCollection AddChatGPTServices(this IServiceCollection services)
      {
         services.AddHttpClient();
         services.AddLogging();
         services.AddTransient<IMessage, Message>();
         services.AddTransient<IMessageRelay, MessageRelay>();
         return services;
      }
   }
}
