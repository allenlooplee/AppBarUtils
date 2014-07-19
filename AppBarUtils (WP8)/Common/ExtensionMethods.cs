//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
    public static class ExtensionMethods
    {
        public static IApplicationBarMenuItem FindItem(this IApplicationBar appBar, AppBarItemType type, string id)
        {
            // Fixed the issue reported at https://appbarutils.codeplex.com/workitem/7248
            //if (appBar == null)
            //{
            //    throw new ArgumentNullException("Application bar not found.");
            //}

            IApplicationBarMenuItem found = null;

            switch (type)
            {
                case AppBarItemType.Button:
                    found = appBar.Buttons.Cast<IApplicationBarMenuItem>().FirstOrDefault(item => item.Text == id);
                    break;
                case AppBarItemType.MenuItem:
                    found = appBar.MenuItems.Cast<IApplicationBarMenuItem>().FirstOrDefault(item => item.Text == id);
                    break;
                default:
                    throw new ArgumentException("Application bar item type not supported.");
            }

            return found;
        }

        public static PhoneApplicationPage FindPage(this DependencyObject reference)
        {
            return reference.GetSelfAndAncestors().OfType<PhoneApplicationPage>().FirstOrDefault();
        }

        public static void AttachTriggers(this DependencyObject target, params System.Windows.Interactivity.TriggerBase[] triggers)
        {
            var triggerCollection = Interaction.GetTriggers(target);
            foreach (var trigger in triggers)
            {
                triggerCollection.Add(trigger);
            }
            target.SetValue(Interaction.TriggersProperty, triggerCollection);
        }

        public static void AttachBehaviors(this DependencyObject target, params System.Windows.Interactivity.Behavior[] behaviors)
        {
            var behaviorCollection = Interaction.GetBehaviors(target);
            foreach (var behavior in behaviors)
            {
                behaviorCollection.Add(behavior);
            }
            target.SetValue(Interaction.BehaviorsProperty, behaviorCollection);
        }
    }
}
