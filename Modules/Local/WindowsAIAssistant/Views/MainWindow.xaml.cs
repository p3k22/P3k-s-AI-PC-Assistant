namespace WindowsAIAssistant.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
   //public MainWindow()
   //{
   //   Debug.Write("WTA");
   //   InitializeComponent();
   //}

   public MainWindow(ChatView chatView)
   {
      InitializeComponent();
      LeftPane.Content = chatView;
   }
}