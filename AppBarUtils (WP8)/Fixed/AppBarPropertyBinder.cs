//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AppBarUtils
{
    // Thanks w0rddriven for contributing the code!
    // Originally this was a trigger. I turned it into a behavior because it does not further invoke any action.
    // For more information, see http://appbarutils.codeplex.com/discussions/275927
    public class AppBarPropertyBinder : Behavior<PhoneApplicationPage>
    {
        IApplicationBar _appBar;

        #region IsVisible dependency property

        public bool IsVisible
        {
            get { return (bool)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(AppBarPropertyBinder), new PropertyMetadata(true, IsVisiblePropertyChanged));

        private void ChangeIsVisible()
        {
            if (_appBar != null)
            {
                _appBar.IsVisible = IsVisible;
            }
        }

        private static void IsVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeIsVisible();
        }

        #endregion

        #region IsMenuEnabled dependency property

        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(MenuEnabledProperty); }
            set { SetValue(MenuEnabledProperty, value); }
        }

        public static readonly DependencyProperty MenuEnabledProperty =
            DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(AppBarPropertyBinder), new PropertyMetadata(true, IsMenuEnabledPropertyChanged));

        private void ChangeIsMenuEnabled()
        {
            if (_appBar != null)
            {
                _appBar.IsMenuEnabled = IsMenuEnabled;
            }
        }

        private static void IsMenuEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeIsMenuEnabled();
        }

        #endregion

        #region Mode dependency property

        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ApplicationBarMode), typeof(AppBarPropertyBinder), new PropertyMetadata(ApplicationBarMode.Default, ModePropertyChanged));

        private void ChangeMode()
        {
            if (_appBar != null)
            {
                _appBar.Mode = Mode;
            }
        }

        private static void ModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeMode();
        }

        #endregion

        #region Opacity dependency property

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(AppBarPropertyBinder), new PropertyMetadata(1.0, OpacityPropertyChanged));

        private void ChangeOpacity()
        {
            if (_appBar != null)
            {
                _appBar.Opacity = Opacity;
            }
        }

        private static void OpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeOpacity();
        }

        #endregion

        #region ForegroundColor dependency property

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(AppBarPropertyBinder), new PropertyMetadata(ForegroundColorPropertyChanged));

        private void ChangeForegroundColor()
        {
            if (_appBar != null)
            {
                _appBar.ForegroundColor = ForegroundColor;
            }
        }

        private static void ForegroundColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeForegroundColor();
        }

        #endregion

        #region BackgroundColor dependency property

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(AppBarPropertyBinder), new PropertyMetadata(BackgroundColorPropertyChanged));

        private void ChangeBackgroundColor()
        {
            if (_appBar != null)
            {
                _appBar.BackgroundColor = BackgroundColor;
            }
        }

        private static void BackgroundColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBarPropertyBinder)sender).ChangeBackgroundColor();
        }

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            _appBar = AssociatedObject.ApplicationBar;

            if (_appBar != null)
            {
                _appBar.IsVisible = IsVisible;
                _appBar.IsMenuEnabled = IsMenuEnabled;
                _appBar.Mode = Mode;
                _appBar.Opacity = Opacity;
                _appBar.ForegroundColor = ForegroundColor;
                _appBar.BackgroundColor = BackgroundColor;
            }
        }
    }
}
