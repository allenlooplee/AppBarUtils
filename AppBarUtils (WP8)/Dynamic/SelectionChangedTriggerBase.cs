//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;

namespace AppBarUtils
{
    public class SelectionChangedTriggerBase<T> : TriggerBase<T> where T : DependencyObject
    {
        public SelectionChangedTriggerBase()
        {
            SelectionMappings = new List<SelectionMapping>();
        }

        public List<SelectionMapping> SelectionMappings { get; private set; }

        private int GetTargetIndex(int selectedIndex)
        {
            var targetIndex = selectedIndex;

            var mapping = SelectionMappings.FirstOrDefault(m => m.SourceIndex == selectedIndex);
            if (mapping != null)
            {
                targetIndex = mapping.TargetIndex;
            }

            return targetIndex;
        }

        protected void ChangeTarget(int selectedIndex)
        {
            InvokeActions(GetTargetIndex(selectedIndex));
        }
    }
}
