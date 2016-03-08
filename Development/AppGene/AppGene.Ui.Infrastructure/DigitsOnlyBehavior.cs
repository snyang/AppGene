using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppGene.Ui.Infrastructure
{
    public static class DigitsOnlyBehavior
    {
        public static bool GetIsDigitOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDigitOnlyProperty);
        }

        public static void SetIsDigitOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDigitOnlyProperty, value);
        }

        public static readonly DependencyProperty IsDigitOnlyProperty =
          DependencyProperty.RegisterAttached("IsDigitOnly",
          typeof(bool), typeof(DigitsOnlyBehavior),
          new PropertyMetadata(false, OnIsDigitOnlyChanged));

        private static void OnIsDigitOnlyChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // ignoring error checking
            var textBox = (TextBox)sender;
            var isDigitOnly = (bool)(e.NewValue);

            if (isDigitOnly)
            { 
                textBox.PreviewTextInput += HandlePreviewTextInput;
                // Catch application and editing commands for this control
                CommandManager.AddPreviewExecutedHandler(textBox, new ExecutedRoutedEventHandler(HandlePreviewExecutedEvent));
            }
            else
            {
                textBox.PreviewTextInput -= HandlePreviewTextInput;
            }
        }

        private static void HandlePreviewExecutedEvent(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                e.Handled = IsInvalid((string)Clipboard.GetDataObject().GetData(typeof(string)));
            }
        }

        private static void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = IsInvalid(e.Text);
        }

        private static bool IsInvalid(string value)
        {
            return value.Any(ch => !Char.IsDigit(ch));
        }
    }


}
