//===============================================================================
// Copyright © 2014 Marcel Marnitz
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Input;

namespace AppBarUtils
{
    public class AppBarToggleItemCommand : AppBarItemCommand
    {
        #region IsChecked dependency property

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(AppBarItemCommand), new PropertyMetadata(false, OnIsCheckedChanged));

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleItemCommand).OnIsCheckedChanged(e);
        }

        private void OnIsCheckedChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _item as IApplicationBarIconButton;

            if (button == null)
            {
                return;
            }

            if ((bool)e.NewValue == true)
            {
                button.IconUri = CheckedIconUri;
                button.Text = CheckedText;

                if (CheckedCommand != null)
                {
                    button.IsEnabled = CheckedCommand.CanExecute(CheckedCommandParameter);
                }
            }
            else
            {
                button.IconUri = IconUri;
                button.Text = Text;

                if (Command != null)
                {
                    button.IsEnabled = Command.CanExecute(CommandParameter);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the ToggleButton
        /// is checked or not
        /// </summary>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        #endregion

        #region CheckedText dependency property

        public static readonly DependencyProperty CheckedTextProperty =
            DependencyProperty.Register("CheckedText", typeof(string), typeof(AppBarToggleItemCommand), new PropertyMetadata(OnCheckedTextChanged));

        private static void OnCheckedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleItemCommand).OnCheckedTextChanged(e);
        }

        private void OnCheckedTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            var button = _item as IApplicationBarIconButton;

            if (button != null && !string.IsNullOrWhiteSpace(newValue) && IsChecked)
            {
                button.Text = newValue;
            }
        }

        /// <summary>
        /// Gets or sets the Text property
        /// for the checked state
        /// </summary>
        public string CheckedText
        {
            get { return (string)GetValue(CheckedTextProperty); }
            set { SetValue(CheckedTextProperty, value); }
        }

        #endregion

        #region CheckedIconUri dependency property

        public static readonly DependencyProperty CheckedIconUriProperty =
            DependencyProperty.Register("CheckedIconUri", typeof(Uri), typeof(AppBarToggleItemCommand), new PropertyMetadata(OnCheckedIconUriChanged));

        private static void OnCheckedIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleItemCommand).OnCheckedIconUriChanged(e);
        }

        private void OnCheckedIconUriChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _item as IApplicationBarIconButton;

            if (button != null && IsChecked)
            {
                button.IconUri = (Uri)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the IconUri property
        /// for the checked state
        /// </summary>
        public Uri CheckedIconUri
        {
            get { return (Uri)GetValue(CheckedIconUriProperty); }
            set { SetValue(CheckedIconUriProperty, value); }
        }

        #endregion

        #region CheckedCommand dependency property

        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(AppBarToggleItemCommand), new PropertyMetadata(OnCheckedCommandChanged));

        private static void OnCheckedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleItemCommand).OnCheckedCommandChanged(e);
        }

        private void OnCheckedCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldCommand = e.OldValue as ICommand;
            if (oldCommand != null)
            {
                oldCommand.CanExecuteChanged -= CanExecuteChanged;
            }

            var newCommand = e.NewValue as ICommand;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += CanExecuteChanged;
                OnIsEnabledChanged();
            }
        }

        /// <summary>
        /// Gets or sets the Command property
        /// for the checked state
        /// </summary>
        public ICommand CheckedCommand
        {
            get { return (ICommand)GetValue(CheckedCommandProperty); }
            set { SetValue(CheckedCommandProperty, value); }
        }

        #endregion

        #region CheckedCommandParameter dependency property

        public static readonly DependencyProperty CheckedCommandParameterProperty =
            DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(AppBarToggleItemCommand), null);

        /// <summary>
        /// Gets or sets the CommandParameter property
        /// for the checked state
        /// </summary>
        public object CheckedCommandParameter
        {
            get { return GetValue(CheckedCommandParameterProperty); }
            set { SetValue(CheckedCommandParameterProperty, value); }
        }

        #endregion

        #region Overrides

        protected override void OnIconUriChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _item as IApplicationBarIconButton;

            if (Type == AppBarItemType.Button && button != null && IconUri != null && !IsChecked)
            {
                button.IconUri = (Uri)e.NewValue;
            }
        }

        protected override void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;

            if (_item != null && !string.IsNullOrWhiteSpace(newValue) && !IsChecked)
            {
                _item.Text = newValue;
            }
        }

        protected override void OnIsEnabledChanged()
        {
            // Fix for the issue reported at http://appbarutils.codeplex.com/discussions/274048
            // Thanks juarola for reporting this issue!
            if (_item != null && Command != null && !IsChecked)
            {
                _item.IsEnabled = Command.CanExecute(CommandParameter);
            }
            else if (_item != null && CheckedCommand != null && IsChecked)
            {
                _item.IsEnabled = CheckedCommand.CanExecute(CheckedCommandParameter);
            }
        }

        protected override void OnItemClick()
        {
            if (!IsChecked && Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
            else if (IsChecked && CheckedCommand != null && CheckedCommand.CanExecute(CheckedCommandParameter))
            {
                CheckedCommand.Execute(CheckedCommandParameter);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (IsChecked)
            {
                var button = _item as IApplicationBarIconButton;

                button.Text = CheckedText;
                button.IconUri = CheckedIconUri;

                if (CheckedCommand != null)
                {
                    button.IsEnabled = CheckedCommand.CanExecute(CheckedCommandParameter);
                }
                else
                {
                    button.IsEnabled = true;
                }
            }
        }

        #endregion
    }
}
