namespace WindowsAIAssistant.ViewModels
{
   using System.ComponentModel;
   using System.Diagnostics;
   using System.Runtime.CompilerServices;
   using System.Windows.Controls;

   using WindowsAIAssistant.Interfaces.IServices;

   public class ChatViewModel : INotifyPropertyChanged
   {
      public Action<ScrollViewer> ScrollViewerMoved = null!;

      private bool _isAllowedSendButton, _fullResponseReceived;

      private bool _isMutingTextToSpeech;

      private CancellationTokenSource _cancellationTokenSource;

      private double _totalTokenCost;

      private int _totalTokenCount;
      //private readonly IChatComposer _chatComposer;
      //private readonly ISpeechComposer _speechComposer;

      private ListView _listView = null!;

      private ScrollViewer _scrollView = null!;

      //private readonly WindowEvents _events;

      public ChatViewModel(
         IChatGPT chatGPT /*IChatComposer chatComposer, ISpeechComposer speechComposer, WindowEvents events*/)
      {
         //_chatComposer = chatComposer;
         //_speechComposer = speechComposer;
         //_events = events;

         //IsAllowedSendButton = true;

         //_events.ChatWindowWidthChanged += OnChatWindowWidthChanged;
         //_events.ScrollViewerToBottom += OnScrollToBottom;

         _cancellationTokenSource = new CancellationTokenSource();

         chatGPT.SendMessageAsync("Waddup my glib glob", cancellationToken: _cancellationTokenSource.Token);
      }

      public event PropertyChangedEventHandler? PropertyChanged;

      public bool IsAllowedSendButton
      {
         get => _isAllowedSendButton;
         set
         {
            _isAllowedSendButton = value;
            OnPropertyChanged();
         }
      }

      public bool IsMutingTextToSpeech
      {
         get => _isMutingTextToSpeech;
         set
         {
            //_isMutingTextToSpeech = value;
            //_speechComposer.Mute(value);
            //OnPropertyChanged();
         }
      }

      //public ObservableCollection<IChatBubble> Messages { get; } = [];

      public double TotalTokenCost
      {
         get => _totalTokenCost;
         private set
         {
            _totalTokenCost = value;
            OnPropertyChanged();
         }
      }

      public int TotalTokenCount
      {
         get => _totalTokenCount;
         private set
         {
            _totalTokenCount = value;
            OnPropertyChanged();
         }
      }

      public static void SetCurrentRTB(RichTextBox richTextBox)
      {
         //if (richTextBox.DataContext is ChatBubble chatBubble)
         //{
         //   chatBubble.RichTextBox = richTextBox;
         //}
      }

      public static void SetTextBoxWidth(RichTextBox tb, double windowWidth)
      {
         //  RichTextBoxHelper.SetTextBoxWidth(tb, windowWidth);
      }

      public async Task SendChatRequestAsync(string input)
      {
         await _cancellationTokenSource.CancelAsync();

         _cancellationTokenSource = new CancellationTokenSource();
         IsAllowedSendButton = false;
         _fullResponseReceived = false;
         Debug.WriteLine($"ChatViewModel.cs;\tSending Chat Request: {input}");

         //Messages.Add(new ChatBubble(0, input, _chatComposer.ChatPrompt.Model));

         //_ = _chatComposer.SendChatRequestAsync(input, _cancellationTokenSource.Token);
         //_ = _speechComposer.StartComposingAsync(_cancellationTokenSource.Token);

         await AddAssistantResponseAsync();
      }

      public void SetUIElements(ListView listView)
      {
         _listView = listView;

         if (_listView.Template.FindName("ListScrollViewer", _listView) is not ScrollViewer scrollView)
         {
            return;
         }

         _scrollView = scrollView;
         _scrollView.ScrollChanged += OnHideLoadMessagesButton;
         _scrollView.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
         _scrollView.Focus();
      }

      private async Task AddAssistantResponseAsync()
      {
         //var model = _chatComposer.ChatPrompt.Model;
         //var token = 0;
         //var isStreaming = true;
         //while (isStreaming)
         //{
         //   try
         //   {
         //      var streamedResponse = _chatComposer.ChatResponse.MessageStream;
         //      isStreaming = !(_chatComposer.ChatResponse.IsResponseComplete && token == streamedResponse.Count);

         //      // Check if we have new messages to process
         //      if (streamedResponse.Count > 0 && token < streamedResponse.Count)
         //      {
         //         if (token == 0)
         //         {
         //            Messages.Add(new ChatBubble(1, streamedResponse[0], model));
         //         }
         //         else
         //         {
         //            Messages[^1].AddTextToRichTextBox(streamedResponse[token]);
         //            if (token == 1)
         //            {
         //               IsAllowedSendButton = true;
         //            }
         //         }

         //         token++;
         //      }

         //      // Exit if the response is complete and we've processed all tokens
         //      if (_chatComposer.ChatResponse.IsResponseComplete)
         //      {
         //         UpdateTokenUsage();
         //      }

         //      // Trigger UI scroll update
         //      OnScrollToBottom();

         //      //Debug.WriteLine($"Token:{token}, Stream:{streamedResponse.Count}");
         //      // Async delay based on message speed, handle cancellation if triggered
         //      await Task.Delay(100 - _chatComposer.ChatPrompt.MessageSpeed, _cancellationTokenSource.Token);
         //   }
         //   catch (TaskCanceledException)
         //   {
         //      // If the cancellation token is triggered, we handle it gracefully and exit
         //      Debug.WriteLine("ChatViewModel.cs\tRequest cancelled due to cancellation token");
         //      return; // Exit the loop as cancellation was requested
         //   }
         //   catch (Exception ex)
         //   {
         //      // Log any unexpected errors, but continue the loop
         //      Debug.WriteLine($"ChatViewModel.cs\tAn error occurred: {ex.Message}");
         //      return;
         //   }
         //}

         IsAllowedSendButton = true;
         Debug.WriteLine("ChatViewModel.cs\tStreaming Complete");
      }

      private void OnChatWindowWidthChanged(double newWidth)
      {
         //ChatWindowHelper.OnChatWindowWidthChanged(_listView, newWidth);
      }

      private void OnHideLoadMessagesButton(object sender, ScrollChangedEventArgs e)
      {
         ScrollViewerMoved.Invoke(_scrollView);
      }

      private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
      {
         PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      private void OnScrollToBottom()
      {
         _scrollView.ScrollToBottom();
      }

      /// <summary>
      ///    Updates the token usage based on the prompt and completion tokens, and the associated model.
      /// </summary>
      private void UpdateTokenUsage()
      {
         //if (_fullResponseReceived)
         //{
         //   return;
         //}

         //var model = _chatComposer.ChatPrompt.Model;
         //var promptToken = _chatComposer.ChatTokens.PromptTokens;
         //var completionToken = _chatComposer.ChatTokens.CompletionTokens;

         //Messages[^1].SetTagText(1, completionToken);
         //Messages[^2].SetTagText(0, promptToken);

         //// Calculate token usage costs based on the model
         //var inputCost = TokenCounterHelper.PricePerToken_Input(model);
         //var outputCost = TokenCounterHelper.PricePerToken_Output(model);

         //// Update total tokens and costs
         //TotalTokenCount += promptToken + completionToken;
         //TotalTokenCost += (promptToken * inputCost) + (completionToken * outputCost);

         //_fullResponseReceived = true;
      }
   }
}
