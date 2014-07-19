//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Windows;
using System.Windows.Controls;
using Microsoft.Phone.Controls;

namespace AppBarUtils
{
	public class SelectedPanoItemChangedTrigger : SelectionChangedTriggerBase<Panorama>
	{
		protected override void OnAttached()
		{
			base.OnAttached();

			// Insert code that you want to run when the Trigger is attached to an object.
            AssociatedObject.Loaded += AssociatedObject_Loaded;
		}

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeTarget(AssociatedObject.SelectedIndex);
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeTarget(AssociatedObject.SelectedIndex);
        }

		protected override void OnDetaching()
		{
            base.OnDetaching();

			// Insert code that you would want run when the Trigger is removed from an object.
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
		}
	}
}