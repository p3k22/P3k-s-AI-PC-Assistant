namespace WindowsAIAssistant.Views;

using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using WindowsAIAssistant.ViewModels;

/// <summary>
///    Interaction logic for ChatView.xaml
/// </summary>
public partial class ChatView
{
   private readonly ChatViewModel _viewModel;

   public ChatView(ChatViewModel viewModel)
   {
      InitializeComponent();

      _viewModel = viewModel;
      InitiateViewModel();
   }

   private void InitiateViewModel()
   {
      DataContext = _viewModel;

      // Dispatcher.BeginInvoke(new Action(() => { _viewModel.SetUIElements(ChatListView); }), DispatcherPriority.Loaded);

      //_viewModel.ScrollViewerMoved +=
      //   OnHideLoadMessagesButton; // TODO -- Turn this into a bool in the view model and bind it to LoadMessages.visibility
   }

   private void OnHideLoadMessagesButton(ScrollViewer scrollViewer)
   {
      LoadOldMessages.Visibility = scrollViewer.VerticalOffset < 40 ? Visibility.Visible : Visibility.Hidden;
   }

   private void OnLoadMessages_Click(object sender, MouseButtonEventArgs e)
   {
      // Handle loading of old messages
      Debug.WriteLine("iya");
   }

   private void OnPromptSendButton_Click(object sender, RoutedEventArgs e)
   {
      if (SendButton.IsEnabled)
      {
         OnSendMessage(); // This should just call the viewmodel method.
      }
   }

   private void OnPromptTextBox_GotFocus(object sender, RoutedEventArgs e)
   {
      if (PromptTextBox.Text == "Enter your prompt here...")
      {
         PromptTextBox.Text = string.Empty;
         PromptTextBox.Foreground = new SolidColorBrush(Colors.White);
      }
   }

   private void OnPromptTextBox_KeyDown(object sender, KeyEventArgs e)
   {
      if (e.Key == Key.Return)
      {
         if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
         {
            e.Handled = false; // Add a new line
         }
         else
         {
            if (SendButton.IsEnabled)
            {
               e.Handled = true;
               OnSendMessage();
            }
         }
      }
   }

   private void OnPromptTextBox_LostFocus(object sender, RoutedEventArgs e)
   {
      if (string.IsNullOrWhiteSpace(PromptTextBox.Text))
      {
         PromptTextBox.Text = "Enter your prompt here...";
         PromptTextBox.Foreground = new SolidColorBrush(Colors.DarkGray);
      }
   }

   private void OnPromptTextBox_TextChanged(object sender, TextChangedEventArgs e)
   {
      if (sender is TextBox textBox)
      {
         textBox.Height = double.NaN;
         textBox.Measure(new Size(textBox.ActualWidth, double.PositiveInfinity));
         textBox.Height = Math.Min(textBox.MinHeight, textBox.DesiredSize.Height);
      }
   }

   private void OnRichTextBox_Loaded(object sender, RoutedEventArgs e)
   {
      if (sender is RichTextBox richTextBox)
      {
         ChatViewModel.SetCurrentRTB(richTextBox);
         ChatViewModel.SetTextBoxWidth(richTextBox, ActualWidth);
      }
   }

   private void OnRichTextBoxText_Changed(object sender, TextChangedEventArgs e)
   {
      var tb = (sender as RichTextBox)!;
      ChatViewModel.SetTextBoxWidth(tb, ActualWidth);
   }

   private async void OnSendMessage()
   {
      var message = PromptTextBox.Text;

      if (message == "Enter your prompt here..." || string.IsNullOrEmpty(message))
      {
         return;
      }

      PromptTextBox.Text = "";
      await _viewModel.SendChatRequestAsync(message.Trim()); // does this really need to be async or awaited?
      PromptTextBox.Focus();
   }

   private void OnSoundImage_Click(object sender, MouseButtonEventArgs e)
   {
      _viewModel.IsMutingTextToSpeech = !_viewModel.IsMutingTextToSpeech;

      TextToSpeechToggle.Source = _viewModel.IsMutingTextToSpeech == false ?
                                     new BitmapImage(
                                     new Uri("pack://application:,,,/Resources/Images/sound_enabled.png")) :
                                     new BitmapImage(
                                     new Uri("pack://application:,,,/Resources/Images/sound_disabled.png"));
   }
}