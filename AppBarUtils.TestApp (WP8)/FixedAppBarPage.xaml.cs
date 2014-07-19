//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using AppBarUtils.TestApp.ViewModels;
using Microsoft.Phone.Controls;

namespace AppBarUtils.TestApp
{
    public partial class FixedAppBarPage : PhoneApplicationPage
    {
        public FixedAppBarPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                DataContext = new FixedAppBarViewModel();
            }

            base.OnNavigatedTo(e);
        }
    }
}