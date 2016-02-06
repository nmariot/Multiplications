using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ExerciceMultiplications.MVVM
{
    // Code récupéré depuis : http://www.codeproject.com/Articles/42908/Silverlight-Behaviors-and-Triggers-TextBox-Magic.aspx

    public static class TextBoxExtension
    {
        private static readonly List<Key> _controlKeys = new List<Key>
                                                            {
                                                                Key.Back,
                                                                Key.CapsLock,
                                                                Key.LeftCtrl,
                                                                Key.RightCtrl,
                                                                Key.Down,
                                                                Key.End,
                                                                Key.Enter,
                                                                Key.Escape,
                                                                Key.Home,
                                                                Key.Insert,
                                                                Key.Left,
                                                                Key.PageDown,
                                                                Key.PageUp,
                                                                Key.Right,
                                                                Key.LeftShift,
                                                                Key.RightShift,
                                                                Key.Tab,
                                                                Key.Up
                                                            };

        private static bool _IsDigit(Key key)
        {
            bool shiftKey = (Keyboard.Modifiers & ModifierKeys.Shift) != 0;
            bool retVal;
            if (key >= Key.D0 && key <= Key.D9 && shiftKey)
            {
                retVal = true;
            }
            else
            {
                retVal = key >= Key.NumPad0 && key <= Key.NumPad9;
            }
            return retVal;
        }

        public static bool GetIsPositiveNumericFilter(DependencyObject src)
        {
            return (bool)src.GetValue(IsPositiveNumericFilterProperty);
        }

        public static void SetIsPositiveNumericFilter(DependencyObject src, bool value)
        {
            src.SetValue(IsPositiveNumericFilterProperty, value);
        }

        public static DependencyProperty IsPositiveNumericFilterProperty =
            DependencyProperty.RegisterAttached(
            "IsPositiveNumericFilter", typeof(bool), typeof(TextBoxExtension),
            new PropertyMetadata(false, IsPositiveNumericFilterChanged));

        public static void IsPositiveNumericFilterChanged
            (DependencyObject src, DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                if ((bool)args.NewValue)
                {
                    textBox.KeyDown += _TextBoxPositiveNumericKeyDown;
                }
            }
        }

        static void _TextBoxPositiveNumericKeyDown(object sender, KeyEventArgs e)
        {
            bool handled = true;

            if (e.Key == Key.Tab || _IsDigit(e.Key))
            {
                handled = false;
            }

            e.Handled = handled;
        }

        public static bool GetIsBoundOnChange(DependencyObject src)
        {
            return (bool)src.GetValue(IsBoundOnChangeProperty);
        }

        public static void SetIsBoundOnChange(DependencyObject src, bool value)
        {
            src.SetValue(IsBoundOnChangeProperty, value);
        }

        public static DependencyProperty IsBoundOnChangeProperty =
                    DependencyProperty.RegisterAttached(
            "IsBoundOnChange", typeof(bool), typeof(TextBoxExtension),
            new PropertyMetadata(false, IsBoundOnChangeChanged));

        public static void IsBoundOnChangeChanged(DependencyObject src,
                    DependencyPropertyChangedEventArgs args)
        {
            if (src != null && src is TextBox)
            {
                TextBox textBox = src as TextBox;

                if ((bool)args.NewValue)
                {
                    textBox.TextChanged += _TextBoxTextChanged;
                }
            }
        }

        static void _TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                TextBox tb = sender as TextBox;
                BindingExpression binding = tb.GetBindingExpression(TextBox.TextProperty);
                if (binding != null)
                {
                    binding.UpdateSource();
                }
            }
        }
    }
}
