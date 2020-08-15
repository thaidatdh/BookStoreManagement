using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookStoreManagement.UI
{
   public class DataGridHelper
   {
      public static TextDecorationCollection GetTextDecoration(DependencyObject obj)
      {
         return (TextDecorationCollection)obj.GetValue(TextDecorationProperty);
      }

      public static void SetTextDecoration(DependencyObject obj, TextDecorationCollection value)
      {
         obj.SetValue(TextDecorationProperty, value);
      }

      public static readonly DependencyProperty TextDecorationProperty =
        DependencyProperty.RegisterAttached("TextDecoration", typeof(TextDecorationCollection), typeof(DataGridHelper), new FrameworkPropertyMetadata(
          null,
          FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits,
          new PropertyChangedCallback((d, e) =>
          {
             TextBlock textBlock = d as TextBlock;
             if (textBlock != null)
             {
                textBlock.TextDecorations = e.NewValue as TextDecorationCollection;
             }
          })));
   }
}
