//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using Microsoft.Phone.Controls;
using System.ComponentModel;
using AppBarUtils.TestApp.ViewModels;

namespace AppBarUtils.TestApp
{
    public partial class DynamicAppBarPhonePage : PhoneApplicationPage
    {
        public DynamicAppBarPhonePage()
        {
            InitializeComponent();

            DataContext = new DynamicAppBarViewModel();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            var viewModel = (DynamicAppBarViewModel)DataContext;
            if (viewModel.IsSelecting)
            {
                viewModel.IsSelecting = false;
                e.Cancel = true;
            }
        }
    }
}