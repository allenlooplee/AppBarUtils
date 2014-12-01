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
    public class AppBarToggleButton : AppBarButton
    {
        #region IsChecked dependency property

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(AppBarToggleButton), new PropertyMetadata(false, OnIsCheckedChanged));

        private static void OnIsCheckedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleButton).OnIsCheckedChanged(e);
        }

        private void OnIsCheckedChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _applicationBarItem as IApplicationBarIconButton;

            if (button == null)
            {
                return;
            }

            if ((bool)e.NewValue == true)
            {
                button.IconUri = CheckedIconUri;
                button.Text = CheckedText;
                button.IsEnabled = CheckedIsEnabled;

                if (CheckedCommand != null)
                {
                    button.IsEnabled = CheckedCommand.CanExecute(CheckedCommandParameter);
                }
            }
            else
            {
                button.IconUri = IconUri;
                button.Text = Text;
                button.IsEnabled = IsEnabled;

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
            DependencyProperty.Register("CheckedText", typeof(string), typeof(AppBarToggleButton), new PropertyMetadata(OnCheckedTextChanged));

        private static void OnCheckedTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleButton).OnCheckedTextChanged(e);
        }

        private void OnCheckedTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            var button = _applicationBarItem as IApplicationBarIconButton;

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
            DependencyProperty.Register("CheckedIconUri", typeof(Uri), typeof(AppBarToggleButton), new PropertyMetadata(OnCheckedIconUriChanged));

        private static void OnCheckedIconUriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleButton).OnCheckedIconUriChanged(e);
        }

        private void OnCheckedIconUriChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _applicationBarItem as IApplicationBarIconButton;

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

        #region CheckedIsEnabled dependency property

        public static readonly DependencyProperty CheckedIsEnabledProperty =
            DependencyProperty.Register("CheckedIsEnabled", typeof(bool), typeof(AppBarToggleButton), new PropertyMetadata(true, OnCheckedIsEnabledChanged));

        private static void OnCheckedIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleButton).OnCheckedIsEnabledChanged(e);
        }

        private void OnCheckedIsEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _applicationBarItem as IApplicationBarIconButton;

            if (button != null && IsChecked)
            {
                button.IsEnabled = (bool)e.NewValue;
            }
        }

        /// <summary>
        /// Gets or sets the IsEnabled property
        /// for the checked state
        /// </summary>
        public bool CheckedIsEnabled
        {
            get { return (bool)GetValue(CheckedIsEnabledProperty); }
            set { SetValue(CheckedIsEnabledProperty, value); }
        }

        #endregion

        #region CheckedCommand dependency property 

        public static readonly DependencyProperty CheckedCommandProperty =
            DependencyProperty.Register("CheckedCommand", typeof(ICommand), typeof(AppBarToggleButton), new PropertyMetadata(OnCheckedCommandChanged));

        private static void OnCheckedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AppBarToggleButton).OnCheckedCommandChanged(e);
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
                if (IsChecked)
                {
                    IsEnabled = newCommand.CanExecute(CommandParameter);
                }
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
            DependencyProperty.Register("CheckedCommandParameter", typeof(object), typeof(AppBarToggleButton), null);

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

        protected override void OnIsEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            var button = _applicationBarItem as IApplicationBarIconButton;

            if (button != null && !IsChecked)
            {
                button.IsEnabled = (bool)e.NewValue;
            }
        }

        protected override void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;
            var button = _applicationBarItem as IApplicationBarIconButton;

            if (button != null && !string.IsNullOrWhiteSpace(newValue) && !IsChecked)
            {
                button.Text = newValue;
            }
        }

        protected override void OnIconUriChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as Uri;
            var button = _applicationBarItem as IApplicationBarIconButton;

            if (button != null && !IsChecked)
            {
                button.IconUri = newValue;
            }
        }

        protected override void CanExecuteChanged(object sender, EventArgs e)
        {
            if (!IsChecked)
            {
                base.CanExecuteChanged(sender, e);
            }
            else if (CheckedCommand != null)
            {
                CheckedIsEnabled = CheckedCommand.CanExecute(CheckedCommandParameter);
            }
        }

        protected override void SubscribeClickEvent()
        {
            _applicationBarItem.Click +=
                (o, e) =>
                {
                    if (!IsChecked && Command != null && Command.CanExecute(CommandParameter))
                    {
                        Command.Execute(CommandParameter);
                    }
                    else if (IsChecked && CheckedCommand != null && CheckedCommand.CanExecute(CheckedCommandParameter))
                    {
                        CheckedCommand.Execute(CheckedCommandParameter);
                    }

                    if (_manualTrigger != null && Actions.Count > 0)
                    {
                        _manualTrigger.Trigger(null);
                    }
                };
        }

        #endregion
    }
}
