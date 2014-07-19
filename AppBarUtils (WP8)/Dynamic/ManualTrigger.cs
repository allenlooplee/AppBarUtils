//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Windows;
using System.Windows.Interactivity;

namespace AppBarUtils.Dynamic
{
    // Actions can only be invoked through triggers
    // due to the restriction of accessibility.
    // ManualTrigger solves this issue by exposing
    // a Trigger method for the outside to invoke
    // actions through the InvokeActions method.
    internal class ManualTrigger : TriggerBase<DependencyObject>
    {
        public void Trigger(object parameter)
        {
            InvokeActions(parameter);
        }
    }
}
