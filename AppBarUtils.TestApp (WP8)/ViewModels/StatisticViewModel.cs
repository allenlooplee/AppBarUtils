//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System;

namespace AppBarUtils.TestApp.ViewModels
{
    public class StatisticViewModel
    {
        public StatisticViewModel(string hitCount, string timeCount)
        {
            HitCount = hitCount;
            TimeCount = timeCount;
            Footer = String.Format("Computed at {0}", DateTime.Now.ToString());
        }

        public string HitCount { get; private set; }
        public string TimeCount { get; private set; }
        public string Footer { get; private set; }
    }
}
