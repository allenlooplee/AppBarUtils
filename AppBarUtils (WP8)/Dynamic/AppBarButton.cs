//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Windows;
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
    public class AppBarButton : AppBarItemBase
    {
        public AppBarButton()
        {
            _applicationBarItem = new ApplicationBarIconButton();
            SubscribeClickEvent();
        }

        #region IconUri dependency property

        // Use this property to bind the IconUri of an app bar button/menu item to a property of the view model.
        public Uri IconUri
        {
            get { return (Uri)GetValue(IconUriProperty); }
            set { SetValue(IconUriProperty, value); }
        }

        public static readonly DependencyProperty IconUriProperty =
            DependencyProperty.Register("IconUri", typeof(Uri), typeof(AppBarButton), new PropertyMetadata(IconUriPropertyChanged));

        private void ChangeIconUri()
        {
            if (IconUri != null)
            {
                ((IApplicationBarIconButton)_applicationBarItem).IconUri = IconUri;
            }
        }

        private static void IconUriPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarButton)sender).ChangeIconUri();
        }

        #endregion

        public IApplicationBarIconButton ToApplicationBarIconButton()
        {
            var applicationBarIconButton = (IApplicationBarIconButton)_applicationBarItem;

            // Fixed the issue reported at https://appbarutils.codeplex.com/workitem/7200 and https://appbarutils.codeplex.com/discussions/429123
            if (applicationBarIconButton.Text == null)
            {
                applicationBarIconButton.Text = "dummy";
            }

            // Fixed an issue similar to the above.
            if (applicationBarIconButton.IconUri == null)
            {
                applicationBarIconButton.IconUri = new Uri("dummy.png", UriKind.Relative);
            }

            return applicationBarIconButton;
        }
    }
}
