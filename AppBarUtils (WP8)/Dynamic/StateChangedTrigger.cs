//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;
using System.Windows.Interactivity;

namespace AppBarUtils
{
	public class StateChangedTrigger : TriggerBase<FrameworkElement>
	{
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObject_Loaded;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            _ready = true;
            ChangeTarget(State);
        }

        public object State
        {
            get { return (object)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(
            "State",
            typeof(object),
            typeof(StateChangedTrigger),
            new PropertyMetadata(HandleStatePropertyChanged));

        private static void HandleStatePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((StateChangedTrigger)sender).ChangeTarget(e.NewValue);
        }

        private void ChangeTarget(object state)
        {
            if (_ready && state != null)
            {
                int targetIndex = 0;

                if (state is int)
                {
                    targetIndex = (int)state;
                }
                // false -> 0
                // true -> 1
                else if (state is bool)
                {
                    targetIndex = (bool)state ? 1 : 0;
                }
                // Only int underlying type is supported.
                else if (state is Enum && Enum.GetUnderlyingType(state.GetType()) == typeof(int))
                {
                    targetIndex = (int)state;
                }
                //else if (state is Enum && Enum.GetUnderlyingType(state.GetType()) == typeof(byte))
                //{
                //    targetIndex = (byte)state;
                //}
                else if (state is string)
                {
                    Int32.TryParse(state as string, out targetIndex);
                }

                InvokeActions(targetIndex);
            }
        }

        private bool _ready;
	}
}