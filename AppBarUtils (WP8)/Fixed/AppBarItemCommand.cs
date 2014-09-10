using Microsoft.Phone.Shell;
//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;
using System.Windows.Input;

namespace AppBarUtils
{
    public class AppBarItemCommand : AppBarItemBehavior
    {
        #region IsChecked dependency property

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(AppBarItemCommand), new PropertyMetadata(false, OnIsCheckedChanged));

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarItemCommand).OnIsCheckedChanged(e);
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

        #region Command dependency property

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AppBarItemCommand), new PropertyMetadata(OnCommandChanged));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region CommandParameter dependency property

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(AppBarItemCommand), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        #endregion

        #region CheckedText dependency property

        public static readonly DependencyProperty CheckedTextProperty =
            DependencyProperty.Register("CheckedText", typeof(string), typeof(AppBarItemCommand), new PropertyMetadata(OnCheckedTextChanged));

        private static void OnCheckedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarItemCommand).OnCheckedTextChanged(e);
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
            DependencyProperty.Register("CheckedIconUri", typeof(Uri), typeof(AppBarItemCommand), new PropertyMetadata(OnCheckedIconUriChanged));

        private static void OnCheckedIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarItemCommand).OnCheckedIconUriChanged(e);
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
            DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(AppBarItemCommand), new PropertyMetadata(OnCheckedCommandChanged));

        private static void OnCheckedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarItemCommand).OnCheckedCommandChanged(e);
        }

        private void OnCheckedCommandChanged(DependencyPropertyChangedEventArgs e)
        {
            ChangeIsEnabled();
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
            DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(AppBarItemCommand), null);

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

        protected override void OnAttached()
        {
            base.OnAttached();

            // The target app bar item was obtain in the base method.
            if (_item != null)
            {
                // Fix for the issue reported at http://appbarutils.codeplex.com/discussions/274048
                // Thanks Loongzxl for reporting this issue!
                // When this utils is used together with MVVM Light Toolkit, especially involing view model locator,
                // binding to command object in XAML will take place before calling OnAttached method.
                // Calling ChangeIsEnabled method here makes sure that a valid app bar item is obtained.
                ChangeIsEnabled();
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (Command != null)
            {
                Command.CanExecuteChanged -= CanExecuteChanged;
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

        protected void ChangeIsEnabled()
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

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            ChangeIsEnabled();
        }

        private void OnCommandChanged(DependencyPropertyChangedEventArgs e)
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
                ChangeIsEnabled();
            }
        }

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemCommand)sender).OnCommandChanged(e);
        }

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

        #endregion
    }
}
