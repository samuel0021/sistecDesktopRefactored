using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace sistecDesktopRefactored.Helpers
{
    public static class ButtonHelper
    {
        // HoverBackground
        public static readonly DependencyProperty HoverBackgroundProperty =
            DependencyProperty.RegisterAttached(
                "HoverBackground",
                typeof(Brush),
                typeof(ButtonHelper),
                new PropertyMetadata(null));

        public static void SetHoverBackground(DependencyObject element, Brush value)
        {
            element.SetValue(HoverBackgroundProperty, value);
        }

        public static Brush GetHoverBackground(DependencyObject element)
        {
            return (Brush)element.GetValue(HoverBackgroundProperty);
        }

        // SelectedTag
        public static readonly DependencyProperty SelectedTagProperty =
            DependencyProperty.RegisterAttached(
                "SelectedTag",
                typeof(string),
                typeof(ButtonHelper),
                new PropertyMetadata(null));

        public static void SetSelectedTag(DependencyObject element, string value) =>
            element.SetValue(SelectedTagProperty, value);

        public static string GetSelectedTag(DependencyObject element) => 
            (string)element.GetValue(SelectedTagProperty);
    }
}
