namespace WindowsAIAssistant;

using ChatGPT.Extensions;

using Microsoft.Extensions.DependencyInjection;

using TextToSpeech.Extensions;

using WindowsAIAssistant.Interfaces;
using WindowsAIAssistant.Interfaces.IServices;
using WindowsAIAssistant.Services;
using WindowsAIAssistant.ViewModels;
using WindowsAIAssistant.Views;

using ServiceCollection = Microsoft.Extensions.DependencyInjection.ServiceCollection;

/// <summary>
///    Interaction logic for App.xaml
/// </summary>
public partial class App
{
   public App()
   {
      var serviceProvider = AppServiceProvider;
      var win = serviceProvider.GetRequiredService<MainWindow>();
      win.Show();
   }

   private static ServiceProvider AppServiceProvider
   {
      get
      {
         var serviceCollection = new ServiceCollection();
         serviceCollection.AddHttpClient();
         serviceCollection.AddLogging();
         serviceCollection.AddTransient<ChatViewModel>();
         serviceCollection.AddTransient<ChatView>();

         serviceCollection.AddChatGPTServices();
         serviceCollection.AddSingleton<IChatGPT, ChatGPT>();

         serviceCollection.AddSingleton<ITextToSpeech, TextToSpeech>();
         serviceCollection.AddTextToSpeechServices();

         serviceCollection.AddSingleton<MyLogger.Logger>();
         serviceCollection.AddSingleton<MainWindow>();
         return serviceCollection.BuildServiceProvider();
      }
   }
}