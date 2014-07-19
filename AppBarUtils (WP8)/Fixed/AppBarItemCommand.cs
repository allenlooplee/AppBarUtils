﻿//===============================================================================
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
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AppBarItemCommand), new PropertyMetadata(CommandPropertyChanged));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(AppBarItemCommand), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

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
            if (Command != null && Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        protected void ChangeIsEnabled()
        {
            // Fix for the issue reported at http://appbarutils.codeplex.com/discussions/274048
            // Thanks juarola for reporting this issue!
            if (_item != null && Command != null)
            {
                _item.IsEnabled = Command.CanExecute(CommandParameter);
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

        private static void CommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemCommand)sender).OnCommandChanged(e);
        }
    }
}
