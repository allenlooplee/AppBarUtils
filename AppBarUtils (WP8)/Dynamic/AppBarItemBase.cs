//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;
using Microsoft.Phone.Shell;
using System.Windows.Markup;
using AppBarUtils.Dynamic;
using System.Collections.Specialized;

namespace AppBarUtils
{
    [ContentProperty("Actions")]
    public abstract class AppBarItemBase : DependencyObject
    {
        public AppBarItemBase()
        {
            Actions = new DependencyObjectCollection<System.Windows.Interactivity.TriggerAction>();
        }

        #region Text dependency property

        // Use this property to bind the Text of an app bar button/menu item to a property of the view model.
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AppBarItemBase), new PropertyMetadata(OnTextChanged));

        protected virtual void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            var newValue = e.NewValue as string;

            if (!String.IsNullOrWhiteSpace(newValue))
            {
                _applicationBarItem.Text = newValue;
            }
        }

        private static void OnTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemBase)sender).OnTextChanged(e);
        }

        #endregion

        #region IsEnabled dependency property

        // true if the button is enabled; otherwise, false. The default value is true,
        // the same as that of ApplicationBarIconButton/ApplicationBarMenuItem.
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(AppBarItemBase), new PropertyMetadata(true, OnIsEnabledChanged));

        protected virtual void OnIsEnabledChanged(DependencyPropertyChangedEventArgs e)
        {
            _applicationBarItem.IsEnabled = (bool)e.NewValue;
        }

        private static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemBase)sender).OnIsEnabledChanged(e);
        }

        #endregion

        #region Command dependency property

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(AppBarItemBase), new PropertyMetadata(CommandPropertyChanged));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(AppBarItemBase), null);

        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected virtual void CanExecuteChanged(object sender, EventArgs e)
        {
            IsEnabled = Command.CanExecute(CommandParameter);
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
                IsEnabled = newCommand.CanExecute(CommandParameter);
            }
        }

        private static void CommandPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarItemBase)sender).OnCommandChanged(e);
        }

        #endregion

        #region Actions dependency property

        public DependencyObjectCollection<System.Windows.Interactivity.TriggerAction> Actions
        {
            get { return (DependencyObjectCollection<System.Windows.Interactivity.TriggerAction>)GetValue(ActionsProperty); }
            set { SetValue(ActionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Actions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionsProperty =
            DependencyProperty.Register("Actions", typeof(DependencyObjectCollection<System.Windows.Interactivity.TriggerAction>), typeof(AppBarItemBase), new PropertyMetadata(HandleActionsPropertyChanged));

        private static void HandleActionsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var appBarItem = (AppBarItemBase)sender;

            var oldActions = e.OldValue as DependencyObjectCollection<System.Windows.Interactivity.TriggerAction>;
            if (oldActions != null)
            {
                appBarItem.HandleOldActions(oldActions);
            }

            var newActions = e.NewValue as DependencyObjectCollection<System.Windows.Interactivity.TriggerAction>;
            if (newActions != null)
            {
                appBarItem.HandleNewActions(newActions);
            }
        }

        private void HandleNewActions(DependencyObjectCollection<System.Windows.Interactivity.TriggerAction> actions)
        {
            if (_manualTrigger == null)
            {
                _manualTrigger = new ManualTrigger();
                this.AttachTriggers(_manualTrigger);
            }

            _manualTrigger.Actions.Clear();
            foreach (var action in actions)
            {
                _manualTrigger.Actions.Add(action);
            }

            actions.CollectionChanged += Actions_CollectionChanged;
        }

        private void HandleOldActions(DependencyObjectCollection<System.Windows.Interactivity.TriggerAction> actions)
        {
            actions.CollectionChanged -= Actions_CollectionChanged;
        }

        private void Actions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var action = e.NewItems[0] as System.Windows.Interactivity.TriggerAction;
                _manualTrigger.Actions.Add(action);
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var action = e.OldItems[0] as System.Windows.Interactivity.TriggerAction;
                _manualTrigger.Actions.Remove(action);
            }
        }

        internal ManualTrigger _manualTrigger;

        #endregion

        protected IApplicationBarMenuItem _applicationBarItem;

        protected virtual void SubscribeClickEvent()
        {
            _applicationBarItem.Click +=
                (o, e) =>
                {
                    if (Command != null && Command.CanExecute(CommandParameter))
                    {
                        Command.Execute(CommandParameter);
                    }

                    if (_manualTrigger != null && Actions.Count > 0)
                    {
                        _manualTrigger.Trigger(null);
                    }
                };
        }
    }
}
