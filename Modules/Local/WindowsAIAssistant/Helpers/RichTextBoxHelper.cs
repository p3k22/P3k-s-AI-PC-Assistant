namespace WindowsAIAssistant.Helpers;

using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

public static class RichTextBoxHelper
{
   public static void AddInlineText(Paragraph paragraph, string text, bool isBold, bool isUnderline)
   {
      if (!string.IsNullOrEmpty(text))
      {
         var run = new Run(text);

         // Apply underline if needed
         if (isUnderline)
         {
            run.TextDecorations = TextDecorations.Underline;
         }

         // If bold is needed, wrap the Run inside a Bold element
         if (isBold)
         {
            var boldInline = new Bold(run); // Wrap the run inside Bold
            paragraph.Inlines.Add(boldInline);
         }
         else
         {
            // Add the run directly if no bold is needed
            paragraph.Inlines.Add(run);
         }
      }
   }

   public static string ConvertDocumentToString(FlowDocument document)
   {
      var range = new TextRange(document.ContentStart, document.ContentEnd);
      using (var ms = new MemoryStream())
      {
         range.Save(ms, DataFormats.Xaml);
         return Encoding.UTF8.GetString(ms.ToArray());
      }
   }

   public static FlowDocument ConvertStringToDocument(string xamlString)
   {
      if (string.IsNullOrEmpty(xamlString))
      {
         return new FlowDocument();
      }

      var document = new FlowDocument();
      var range = new TextRange(document.ContentStart, document.ContentEnd);
      using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(xamlString)))
      {
         range.Load(ms, DataFormats.Xaml);
      }

      return document;
   }

   public static int FindNextMarkerIndex(int underlineIndex, int boldIndex, int textLength)
   {
      if (underlineIndex == -1)
      {
         return boldIndex == -1 ? textLength : boldIndex;
      }

      return boldIndex == -1 ? underlineIndex : Math.Min(underlineIndex, boldIndex); // Return the closest marker
   }

   public static string GetPlainTextFromRichTextBox(RichTextBox richTextBox)
   {
      if (richTextBox.Document != null)
      {
         var textRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
         return textRange.Text;
      }

      return string.Empty;
   }

   /// <summary>
   ///    Sets the RichTextBox width to the max allowed width based on windowSize
   /// </summary>
   /// <param name="tb"></param>
   /// <param name="windowWidth"></param>
   public static void SetTextBoxWidth(RichTextBox tb, double windowWidth)
   {
      var desiredWidth = GetDesiredWidth(tb, windowWidth);

      // Debug.WriteLine("Setting TB Width");
      // Resize only if the desired width is different from the current width
      if (tb.Document.PageWidth != desiredWidth)
      {
         tb.Document.PageWidth = desiredWidth;
         tb.UpdateLayout();
      }
   }

   /// <summary>
   ///    Gets a width that fits the content bound by a min and max width
   /// </summary>
   /// <param name="richTextBox"></param>
   /// <param name="windowWidth"></param>
   /// <returns></returns>
   private static double GetDesiredWidth(RichTextBox richTextBox, double windowWidth)
   {
      var text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
      var typeFace = new Typeface(
      richTextBox.FontFamily,
      richTextBox.FontStyle,
      richTextBox.FontWeight,
      richTextBox.FontStretch);
      var formattedText = new FormattedText(
      text,
      CultureInfo.CurrentCulture,
      FlowDirection.LeftToRight,
      typeFace,
      richTextBox.FontSize,
      Brushes.Black,
      new NumberSubstitution(),
      1);

      var maxWidth = windowWidth * .5;
      var textWidth = formattedText.Width + 18;
      return Math.Min(textWidth, maxWidth);
   }
}