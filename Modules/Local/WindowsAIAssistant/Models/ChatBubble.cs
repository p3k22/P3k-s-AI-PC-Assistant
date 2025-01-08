namespace WindowsAIAssistant.Models;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

using WindowsAIAssistant.Helpers;
using WindowsAIAssistant.Interfaces.IModels;

public class ChatBubble : IChatBubble
{
   /// <summary>
   ///    Creates a new Message Bubble that displays a message along with a tag above it
   /// </summary>
   /// <param name="messageType">0=User, 1=Assistant, 2=System</param>
   public ChatBubble(int messageType, string message, int gptLlmIndex)
   {
      _richTextBox = null!;
      _gptLlm = gptLlmIndex;

      var systemKeyword = "[system]";
      if (message.StartsWith(systemKeyword, StringComparison.OrdinalIgnoreCase))
      {
         message = message[systemKeyword.Length..].Trim();
         messageType = 2;
      }

      AddTextToRichTextBox(message);

      switch (messageType)
      {
         case 1:
            BackgroundColour_Message = new SolidColorBrush(Color.FromRgb(120, 90, 130));
            FontColour_Message = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            FontSize_Message = 14;
            CornerRadius_Message = 10;

            BackgroundColour_Tags = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            FontColour_Tags = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            FontSize_Tags = 10;

            HorizAlignment = HorizontalAlignment.Left;
            Text_Tags =
               $"---------- <Assistant> {DateTime.Now:dd/MM/yy H:mm} Token Use: Calculating. : Token Cost Calculating.. ----------";
            break;
         case 2:
            BackgroundColour_Message = new SolidColorBrush(Color.FromRgb(90, 90, 90));
            FontColour_Message = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            FontSize_Message = 14;
            CornerRadius_Message = 10;

            BackgroundColour_Tags = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            FontColour_Tags = new SolidColorBrush(Color.FromRgb(140, 140, 220));
            FontSize_Tags = 10;

            HorizAlignment = HorizontalAlignment.Center;
            Text_Tags =
               $"---------- <System> {DateTime.Now:dd/MM/yy H:mm} Token Use: Calculating. : Token Cost Calculating.. ----------";
            break;
         default:
            BackgroundColour_Message = new SolidColorBrush(Color.FromRgb(70, 130, 180));
            FontColour_Message = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            FontSize_Message = 14;
            CornerRadius_Message = 10;

            BackgroundColour_Tags = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            FontColour_Tags = new SolidColorBrush(Color.FromRgb(180, 180, 180));
            FontSize_Tags = 10;

            Text_Tags =
               $"---------- <User> {DateTime.Now:dd/MM/yy H:mm} Token Use: Calculating. : Token Cost Calculating.. ----------";

            break;
      }

      BackgroundColour_Tags = new SolidColorBrush(Color.FromArgb(1, 1, 1, 1));
   }

   public string RichTextDocumentToString => RichTextBoxHelper.GetPlainTextFromRichTextBox(RichTextBox);

   public async void AddTextToRichTextBox(string inputText)
   {
      while (RichTextBox is null)
      {
         await Task.Delay(2);
      }

      // Get the last paragraph from the RichTextBox, or create a new one if none exists
      if (RichTextBox.Document.Blocks.LastBlock is Paragraph paragraph)
      {
         // If the last block is a paragraph, we continue adding to it
         AddTextWithFormatting(paragraph, inputText);
      }
      else
      {
         // Create a new paragraph if no paragraph exists
         paragraph = new Paragraph();
         AddTextWithFormatting(paragraph, inputText);
         RichTextBox.Document.Blocks.Add(paragraph);
      }

      // Save the current document state after adding text
      RichTextDocumentContent = RichTextBoxHelper.ConvertDocumentToString(RichTextBox.Document);
   }

   public void SetTagText(int messageType, int count)
   {
      var rate = messageType == 1 ?
                    TokenCostingsHelper.OutputTokenCost(_gptLlm) :
                    TokenCostingsHelper.InputTokenCost(_gptLlm);
      var cost = count * rate;
      Text_Tags = Text_Tags.Replace("Calculating. ", $"{count}");
      Text_Tags = Text_Tags.Replace("Calculating.. ", $"£{cost:00.0000000}p");
   }

   private static void AddTextWithoutFormatting(Paragraph paragraph, string inputText)
   {
      paragraph.Inlines.Add(inputText);
   }

   private void
      AddTextWithFormatting(Paragraph paragraph, string inputText) // TODO -- This looks like a RTB Helper func
   {
      var currentIndex = 0; // Track our position in the string

      // Loop through the input string to find markers
      while (currentIndex < inputText.Length)
      {
         // Handle underlining mode if currently active
         if (_isUnderline)
         {
            var newLineIndex = inputText.IndexOf('\n', currentIndex);

            if (newLineIndex == -1) // No newline found, keep everything underlined
            {
               RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..], _isBold, true);
               break;
            }

            // Add the text up to the newline as underlined
            RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..newLineIndex], _isBold, true);

            // Add a line break after the newline and stop underlining
            paragraph.Inlines.Add(new LineBreak());
            _isUnderline = false; // Toggle off underline after newline

            currentIndex = newLineIndex + 1; // Move past the newline character
         }
         else if (_isBold)
         {
            // We are in bold mode, find the next "**" to close it
            var boldCloseIndex = inputText.IndexOf("**", currentIndex, StringComparison.Ordinal);

            if (boldCloseIndex == -1) // No closing "**" found, keep everything bold
            {
               RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..], true, _isUnderline);
               break; // No closing bold marker, exit and keep bold state
            }

            // Add the text up to the closing "**" as bold
            RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..boldCloseIndex], true, _isUnderline);
            _isBold = false; // Toggle off bold after closing
            currentIndex = boldCloseIndex + 2; // Move past the closing "**"
         }
         else
         {
            // Handle new marker for underlining (###)
            var underlineStartIndex = inputText.IndexOf("###", currentIndex, StringComparison.Ordinal);
            var boldOpenIndex = inputText.IndexOf("**", currentIndex, StringComparison.Ordinal);

            // Determine which marker comes first
            var nextMarkerIndex = RichTextBoxHelper.FindNextMarkerIndex(
            underlineStartIndex,
            boldOpenIndex,
            inputText.Length);

            if (nextMarkerIndex == inputText.Length) // No markers found, add remaining text as normal
            {
               RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..], false, false);
               break;
            }

            if (nextMarkerIndex == underlineStartIndex)
            {
               // Add text before the underlining marker as normal
               if (underlineStartIndex > currentIndex)
               {
                  RichTextBoxHelper.AddInlineText(
                  paragraph,
                  inputText[currentIndex..underlineStartIndex],
                  false,
                  false);
               }

               // Enable underline mode and move past "###"
               _isUnderline = true;
               currentIndex = underlineStartIndex + 3; // Skip the "###"
            }
            else if (nextMarkerIndex == boldOpenIndex)
            {
               // Add text before the bold marker as normal
               if (boldOpenIndex > currentIndex)
               {
                  RichTextBoxHelper.AddInlineText(paragraph, inputText[currentIndex..boldOpenIndex], false, false);
               }

               // Toggle bold state and move past the "**"
               _isBold = true;
               currentIndex = boldOpenIndex + 2; // Skip the "**"
            }
         }
      }
   }

   private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new(propertyName));
   }

   private void RestoreRichTextBoxContent()
   {
      if (!string.IsNullOrEmpty(RichTextDocumentContent))
      {
         RichTextBox.Document = RichTextBoxHelper.ConvertStringToDocument(RichTextDocumentContent);
      }
   }

// @formatter:off
   #region PrivateFields
   private bool _isBold; // Tracks bold state across calls

   private bool _isUnderline; // Tracks underline state across calls

   private double _cornerRadius_Message;

   private double _fontSize_Message;

   private double _fontSize_Tags;

   private readonly int _gptLlm;

   private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Right;

   private RichTextBox _richTextBox;

   private SolidColorBrush _backgroundColour_Message = null!;

   private SolidColorBrush _backgroundColour_Tags = null!;

   private SolidColorBrush _fontColour_Message = null!;

   private SolidColorBrush _fontColour_Tags = null!;

   private string _richTextDocumentContent = string.Empty;

   private string _text_Tags = string.Empty;
   #endregion

   #region PublicProperties
   public event PropertyChangedEventHandler? PropertyChanged;

   public SolidColorBrush BackgroundColour_Message
   {
      get => _backgroundColour_Message;
      set
      {
         if (_backgroundColour_Message != value)
         {
            _backgroundColour_Message = value;
            OnPropertyChanged();
         }
      }
   }

   public SolidColorBrush BackgroundColour_Tags
   {
      get => _backgroundColour_Tags;
      set
      {
         if (_backgroundColour_Tags != value)
         {
            _backgroundColour_Tags = value;
            OnPropertyChanged();
         }
      }
   }

   public double CornerRadius_Message
   {
      get => _cornerRadius_Message;
      set
      {
         if (_cornerRadius_Message != value)
         {
            _cornerRadius_Message = value;
            OnPropertyChanged();
         }
      }
   }

   public SolidColorBrush FontColour_Message
   {
      get => _fontColour_Message;
      set
      {
         if (_fontColour_Message != value)
         {
            _fontColour_Message = value;
            OnPropertyChanged();
         }
      }
   }

   public SolidColorBrush FontColour_Tags
   {
      get => _fontColour_Tags;
      set
      {
         if (_fontColour_Tags != value)
         {
            _fontColour_Tags = value;
            OnPropertyChanged();
         }
      }
   }

   public double FontSize_Message
   {
      get => _fontSize_Message;
      set
      {
         if (_fontSize_Message != value)
         {
            _fontSize_Message = value;
            OnPropertyChanged();
         }
      }
   }

   public double FontSize_Tags
   {
      get => _fontSize_Tags;
      set
      {
         if (_fontSize_Tags != value)
         {
            _fontSize_Tags = value;
            OnPropertyChanged();
         }
      }
   }

   public HorizontalAlignment HorizAlignment
   {
      get => _horizontalAlignment;
      set
      {
         if (_horizontalAlignment != value)
         {
            _horizontalAlignment = value;
            OnPropertyChanged();
         }
      }
   }

   public RichTextBox RichTextBox
   {
      get => _richTextBox;
      set
      {
         if (_richTextBox != value)
         {
            _richTextBox = value;
            OnPropertyChanged();
            RestoreRichTextBoxContent();
         }
      }
   }

   public string RichTextDocumentContent
   {
      get => _richTextDocumentContent;
      set
      {
         if (_richTextDocumentContent != value)
         {
            _richTextDocumentContent = value;
            OnPropertyChanged();
         }
      }
   }

   public string Text_Tags
   {
      get => _text_Tags;
      set
      {
         if (_text_Tags != value)
         {
            _text_Tags = value;
            OnPropertyChanged();
         }
      }
   }

   #endregion
   // @formatter:on
}