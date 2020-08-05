using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BookStoreManagement.Utils
{
   public class ControlUtils
   {
      public static void ChangeEnableValue(Grid control, bool isEnable = true)
      {
         foreach (UIElement element in control.Children)
         {
            if (element.GetType().Equals(typeof(TextBlock)) || element.GetType().Equals(typeof(Button)))
            {
               continue;
            }
            else if (element.GetType().Equals(typeof(Grid)))
            {
               Grid grid = (Grid)element;
               ChangeEnableValue(grid, isEnable);
            }
            else if (element.GetType().Equals(typeof(StackPanel)))
            {
               StackPanel st = (StackPanel)element;
               ChangeEnableValue(st, isEnable);
            }
            else
            {
               if (element.GetType().Equals(typeof(TextBox)))
               {
                  TextBox tb = (TextBox)element;
                  tb.IsReadOnly = !isEnable;
               }
               else if (element.GetType().Equals(typeof(RichTextBox)))
               {
                  RichTextBox tb = (RichTextBox)element;
                  tb.IsReadOnly = !isEnable;
               }
               else if (element.GetType().Equals(typeof(ComboBox)))
               {
                  ComboBox tb = (ComboBox)element;
                  tb.IsReadOnly = !isEnable;
                  tb.IsHitTestVisible = isEnable;
               }
               else if (element.GetType().Equals(typeof(DatePicker)))
               {
                  DatePicker tb = (DatePicker)element;
                  tb.IsHitTestVisible = isEnable;
               }
               else
               {
                  element.IsEnabled = isEnable;
               }
            }
         }
      }
      public static void ChangeEnableValue(StackPanel control, bool isEnable = true)
      {
         foreach (UIElement element in control.Children)
         {
            if (element.GetType().Equals(typeof(TextBlock))|| element.GetType().Equals(typeof(Label)) || element.GetType().Equals(typeof(Button)))
            {
               continue;
            }
            else if (element.GetType().Equals(typeof(Grid)))
            {
               Grid grid = (Grid)element;
               ChangeEnableValue(grid, isEnable);
            }
            else if (element.GetType().Equals(typeof(StackPanel)))
            {
               StackPanel st = (StackPanel)element;
               ChangeEnableValue(st, isEnable);
            }
            else
            {
               if (element.GetType().Equals(typeof(TextBox)))
               {
                  TextBox tb = (TextBox)element;
                  tb.IsReadOnly = !isEnable;
               }
               else if (element.GetType().Equals(typeof(RichTextBox)))
               {
                  RichTextBox tb = (RichTextBox)element;
                  tb.IsReadOnly = !isEnable;
               }
               else if (element.GetType().Equals(typeof(ComboBox)))
               {
                  ComboBox tb = (ComboBox)element;
                  tb.IsReadOnly = !isEnable;
                  tb.IsHitTestVisible = isEnable;
               }
               else if (element.GetType().Equals(typeof(DatePicker)))
               {
                  DatePicker tb = (DatePicker)element;
                  tb.IsHitTestVisible = isEnable;
               }
               else
               {
                  element.IsEnabled = isEnable;
               }
            }
         }
      }
      
   }
   public static class ControlExtensionUtils
   {
      public static void UpdateContent(this RichTextBox richTextBox, string content)
      {
         richTextBox.Document.Blocks.Clear();
         richTextBox.Document.Blocks.Add(new Paragraph(new Run(content)));
      }
      public static string GetContent(this RichTextBox rtb)
      {
         TextRange textRange = new TextRange(rtb.Document.ContentStart,rtb.Document.ContentEnd);
         return textRange.Text;
      }
   }
}
