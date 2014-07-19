//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
    public class AppBarMenuItem : AppBarItemBase
    {
        public AppBarMenuItem()
        {
            _applicationBarItem = new ApplicationBarMenuItem();
            SubscribeClickEvent();
        }

        public IApplicationBarMenuItem ToApplicationBarMenuItem()
        {
            // Fixed an issue similar to https://appbarutils.codeplex.com/workitem/7200
            if (_applicationBarItem.Text == null)
            {
                _applicationBarItem.Text = "dummy";
            }

            return _applicationBarItem;
        }
    }
}
