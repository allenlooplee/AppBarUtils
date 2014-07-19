//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Markup;
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
	//
	// If you want your Action to target elements other than its parent, extend your class
	// from TargetedTriggerAction instead of from TriggerAction
	//
    [ContentProperty("AppBars")]
	public class SwitchAppBarAction : TriggerAction<DependencyObject>
	{
		public SwitchAppBarAction()
		{
			// Insert code required on object creation below this point.
            AppBars = new DependencyObjectCollection<AppBar>();
		}

		protected override void Invoke(object o)
		{
			// Insert code that defines what the Action will do when triggered/invoked.
            var page = AssociatedObject.FindPage();
            if (page != null && o is int)
            {
                page.ApplicationBar = FindAppBar((int)o);
            }
		}

        public DependencyObjectCollection<AppBar> AppBars
        {
            get { return (DependencyObjectCollection<AppBar>)GetValue(AppBarsProperty); }
            set { SetValue(AppBarsProperty, value); }
        }

        public static readonly DependencyProperty AppBarsProperty =
            DependencyProperty.Register("AppBars", typeof(DependencyObjectCollection<AppBar>), typeof(SwitchAppBarAction), new PropertyMetadata(null));

        private IApplicationBar FindAppBar(int id)
        {
            IApplicationBar applicationBar = null;

            var found = AppBars.FirstOrDefault(appBar => appBar.Id == id);
            if (found != null)
            {
                applicationBar = found.ToApplicationBar();
            }

            return applicationBar;
        }
	}
}