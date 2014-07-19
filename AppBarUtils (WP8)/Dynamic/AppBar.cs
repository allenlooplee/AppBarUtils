//===============================================================================
// Copyright © 2013 Allen Lee
// This code released under the terms of the MIT License (http://appbarutils.codeplex.com/license)
//===============================================================================
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Collections.Specialized;

namespace AppBarUtils
{
    [ContentProperty("Buttons")]
    public class AppBar : DependencyObject
    {
        public AppBar()
        {
            _applicationBar = new ApplicationBar();
            Buttons = new DependencyObjectCollection<AppBarButton>();
            MenuItems = new DependencyObjectCollection<AppBarMenuItem>();
        }

        public int Id { get; set; }

        #region IsVisible dependency property

        public bool IsVisible
        {
            get { return (bool)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
        }

        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register("IsVisible", typeof(bool), typeof(AppBar), new PropertyMetadata(true, IsVisiblePropertyChanged));

        private void ChangeIsVisible()
        {
            _applicationBar.IsVisible = IsVisible;
        }

        private static void IsVisiblePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeIsVisible();
        }

        #endregion

        #region IsMenuEnabled dependency property

        public bool IsMenuEnabled
        {
            get { return (bool)GetValue(MenuEnabledProperty); }
            set { SetValue(MenuEnabledProperty, value); }
        }

        public static readonly DependencyProperty MenuEnabledProperty =
            DependencyProperty.Register("IsMenuEnabled", typeof(bool), typeof(AppBar), new PropertyMetadata(true, IsMenuEnabledPropertyChanged));

        private void ChangeIsMenuEnabled()
        {
            _applicationBar.IsMenuEnabled = IsMenuEnabled;
        }

        private static void IsMenuEnabledPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeIsMenuEnabled();
        }

        #endregion

        #region Mode dependency property

        public ApplicationBarMode Mode
        {
            get { return (ApplicationBarMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ApplicationBarMode), typeof(AppBar), new PropertyMetadata(ApplicationBarMode.Default, ModePropertyChanged));

        private void ChangeMode()
        {
            _applicationBar.Mode = Mode;
        }

        private static void ModePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeMode();
        }

        #endregion

        #region Opacity dependency property

        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register("Opacity", typeof(double), typeof(AppBar), new PropertyMetadata(1.0, OpacityPropertyChanged));

        private void ChangeOpacity()
        {
            _applicationBar.Opacity = Opacity;
        }

        private static void OpacityPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeOpacity();
        }

        #endregion

        #region ForegroundColor dependency property

        public Color ForegroundColor
        {
            get { return (Color)GetValue(ForegroundColorProperty); }
            set { SetValue(ForegroundColorProperty, value); }
        }

        public static readonly DependencyProperty ForegroundColorProperty =
            DependencyProperty.Register("ForegroundColor", typeof(Color), typeof(AppBar), new PropertyMetadata(ForegroundColorPropertyChanged));

        private void ChangeForegroundColor()
        {
            _applicationBar.ForegroundColor = ForegroundColor;
        }

        private static void ForegroundColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeForegroundColor();
        }

        #endregion

        #region BackgroundColor dependency property

        public Color BackgroundColor
        {
            get { return (Color)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly DependencyProperty BackgroundColorProperty =
            DependencyProperty.Register("BackgroundColor", typeof(Color), typeof(AppBar), new PropertyMetadata(BackgroundColorPropertyChanged));

        private void ChangeBackgroundColor()
        {
            _applicationBar.BackgroundColor = BackgroundColor;
        }

        private static void BackgroundColorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((AppBar)sender).ChangeBackgroundColor();
        }

        #endregion

        #region Buttons dependency property

        public DependencyObjectCollection<AppBarButton> Buttons
        {
            get { return (DependencyObjectCollection<AppBarButton>)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }

        public static readonly DependencyProperty ButtonsProperty =
            DependencyProperty.Register(
                "Buttons",
                typeof(DependencyObjectCollection<AppBarButton>),
                typeof(AppBar),
                new PropertyMetadata(HandleButtonsPropertyChanged));

        private static void HandleButtonsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Fixed the issue reported at https://appbarutils.codeplex.com/discussions/444348 and https://appbarutils.codeplex.com/discussions/428628
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                return;
            }

            var appBar = (AppBar)sender;

            var oldButtons = e.OldValue as DependencyObjectCollection<AppBarButton>;
            if (oldButtons != null)
            {
                appBar.HandleOldButtons(oldButtons);
            }

            var newButtons = e.NewValue as DependencyObjectCollection<AppBarButton>;
            if (newButtons != null)
            {
                appBar.HandleNewButtons(newButtons);
            }
        }

        private void HandleNewButtons(DependencyObjectCollection<AppBarButton> buttons)
        {
            _applicationBar.Buttons.Clear();
            foreach (var button in buttons)
            {
                _applicationBar.Buttons.Add(button.ToApplicationBarIconButton());
            }

            buttons.CollectionChanged += Buttons_CollectionChanged;
        }

        private void HandleOldButtons(DependencyObjectCollection<AppBarButton> buttons)
        {
            buttons.CollectionChanged -= Buttons_CollectionChanged;
        }

        private void Buttons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var button = e.NewItems[0] as AppBarButton;
                _applicationBar.Buttons.Add(button.ToApplicationBarIconButton());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var button = e.OldItems[0] as AppBarButton;
                _applicationBar.Buttons.Remove(button.ToApplicationBarIconButton());
            }
        }

        #endregion

        #region MenuItems dependency property

        public DependencyObjectCollection<AppBarMenuItem> MenuItems
        {
            get { return (DependencyObjectCollection<AppBarMenuItem>)GetValue(MenuItemsProperty); }
            set { SetValue(MenuItemsProperty, value); }
        }

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(
                "MenuItems",
                typeof(DependencyObjectCollection<AppBarMenuItem>),
                typeof(AppBar),
                new PropertyMetadata(HandleMenuItemsPropertyChanged));

        private static void HandleMenuItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Fixed an issue similar to https://appbarutils.codeplex.com/discussions/444348 and https://appbarutils.codeplex.com/discussions/428628
            if (System.ComponentModel.DesignerProperties.IsInDesignTool)
            {
                return;
            }

            var appBar = (AppBar)sender;

            var oldMenuItems = e.OldValue as DependencyObjectCollection<AppBarMenuItem>;
            if (oldMenuItems != null)
            {
                appBar.HandleOldMenuItems(oldMenuItems);
            }

            var newMenuItems = e.NewValue as DependencyObjectCollection<AppBarMenuItem>;
            if (newMenuItems != null)
            {
                appBar.HandleNewMenuItems(newMenuItems);
            }
        }

        private void HandleNewMenuItems(DependencyObjectCollection<AppBarMenuItem> menuItems)
        {
            _applicationBar.MenuItems.Clear();
            foreach (var menuItem in menuItems)
            {
                _applicationBar.MenuItems.Add(menuItem.ToApplicationBarMenuItem());
            }

            menuItems.CollectionChanged += MenuItems_CollectionChanged;
        }

        private void HandleOldMenuItems(DependencyObjectCollection<AppBarMenuItem> menuItems)
        {
            menuItems.CollectionChanged -= MenuItems_CollectionChanged;
        }

        private void MenuItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var menuItem = e.NewItems[0] as AppBarMenuItem;
                _applicationBar.MenuItems.Add(menuItem.ToApplicationBarMenuItem());
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var menuItem = e.OldItems[0] as AppBarMenuItem;
                _applicationBar.MenuItems.Remove(menuItem.ToApplicationBarMenuItem());
            }
        }

        #endregion

        private IApplicationBar _applicationBar;
        public IApplicationBar ToApplicationBar()
        {
            return _applicationBar;
        }
    }
}
