namespace WindowsAIAssistant.Interfaces.IModels;

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

/// <summary>
///    Properties for the chat message bubbles
/// </summary>
public interface IChatBubble : INotifyPropertyChanged
{
   // Properties
   SolidColorBrush BackgroundColour_Message { get; set; }

   SolidColorBrush BackgroundColour_Tags { get; set; }

   double CornerRadius_Message { get; set; }

   SolidColorBrush FontColour_Message { get; set; }

   SolidColorBrush FontColour_Tags { get; set; }

   double FontSize_Message { get; set; }

   double FontSize_Tags { get; set; }

   HorizontalAlignment HorizAlignment { get; set; }

   RichTextBox RichTextBox { get; set; }

   string RichTextDocumentContent { get; set; }

   string RichTextDocumentToString { get; }

   string Text_Tags { get; set; }

   void AddTextToRichTextBox(string inputText);

   /// <summary>
   ///    Sets the chat bubbles message tag for displaying time and tokens
   /// </summary>
   /// <param name="messageType">0 is Users Tag, 1 is Assistants Tag</param>
   /// <param name="count">How many tokens each user used</param>
   void SetTagText(int messageType, int count);
}