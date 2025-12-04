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
    }
}
