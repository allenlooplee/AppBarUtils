//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using Microsoft.Phone.Controls;
using System.Windows;

namespace AppBarUtils.TestApp.Utils
{
    public class PivotHelper
    {
        public static bool GetIsLocked(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsLockedProperty);
        }

        public static void SetIsLocked(DependencyObject obj, bool value)
        {
            obj.SetValue(IsLockedProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsLocked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsLockedProperty =
            DependencyProperty.RegisterAttached("IsLocked", typeof(bool), typeof(PivotHelper), new PropertyMetadata(false, HandleIsLockedPropertyChanged));

        private static void HandleIsLockedPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var pivot = sender as Pivot;

            if (pivot != null)
            {
                pivot.IsLocked = (bool)e.NewValue;
            }
        }
    }
}
