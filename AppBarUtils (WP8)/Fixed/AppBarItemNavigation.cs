//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;

namespace AppBarUtils
{
    public class AppBarItemNavigation : AppBarItemBehavior
    {
        public string TargetPage
        {
            get { return (string)GetValue(TargetPageProperty); }
            set { SetValue(TargetPageProperty, value); }
        }

        public static readonly DependencyProperty TargetPageProperty =
            DependencyProperty.Register("TargetPage", typeof(string), typeof(AppBarItemNavigation), new PropertyMetadata(String.Empty));

        protected override void OnItemClick()
        {
            var navigationService = AssociatedObject.NavigationService;

            if (!String.IsNullOrWhiteSpace(TargetPage))
            {
                navigationService.Navigate(new Uri(TargetPage, UriKind.Relative));
            }
            // Use GoBackAction instead if you need to go back
            // unless you have a good reason to enable the below code.
            //else
            //{
            //    if (navigationService.CanGoBack)
            //    {
            //        navigationService.GoBack();
            //    }
            //}
        }
    }
}
