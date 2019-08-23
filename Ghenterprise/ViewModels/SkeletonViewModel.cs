using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Helpers;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Ghenterprise.Views.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Ghenterprise.ViewModels
{
    public class SkeletonViewModel : ViewModelBase
    {
        //TODO: User Authenticatie toevoegen

        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private IEnumerable<WinUI.NavigationViewItem> _menuItems;
        public IEnumerable<WinUI.NavigationViewItem> MenuItems
        {
            get => _menuItems;
            set => Set(ref _menuItems, value);
        }
        private IEnumerable<WinUI.NavigationViewItem> GetMenuItems()
        {
            string[] labels = {
                "Overzicht",
                "Events",
                "Promoties",
                "Mijn Ondernemingen",
                "Mijn Events",
                "Mijn Promoties"
            };
            FontIcon[] icons = {
                new FontIcon() { Glyph = "\uF246" },
                new FontIcon() { Glyph = "\uE787" },
                new FontIcon() { Glyph = "\uE789" },
                new FontIcon() { Glyph = "\uEC06" },
                new FontIcon() { Glyph = "\uE8D1" },
                new FontIcon() { Glyph = "\uE94C" }
            };
            string[] pageViewModels = {
                typeof(OverviewViewModel).FullName,
                typeof(EventViewModel).FullName,
                typeof(PromotionViewModel).FullName,
                typeof(MyEnterpriseViewModel).FullName,
                typeof(MyEventViewModel).FullName,
                typeof(MyPromotionsViewModel).FullName,
            };

            for (int i = 0; i < labels.Length; i++)
            {
                WinUI.NavigationViewItem navigationViewItem = new WinUI.NavigationViewItem();
                navigationViewItem.Content = labels[i];
                navigationViewItem.Icon = icons[i];
                navigationViewItem.SetValue(NavHelper.NavigateToProperty, pageViewModels[i]);
                yield return navigationViewItem;
            }
        }

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _itemInvokedCommand;
        public static NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        public SkeletonViewModel()
        {
        }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }


        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));


        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            _keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigatedFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;

            MenuItems = GetMenuItems().ToArray();
            foreach(WinUI.NavigationViewItem navItem in MenuItems)
            {
                if (navItem.Content.ToString() == "Mijn Ondernemingen")
                {
                    WinUI.NavigationViewItemHeader header = new WinUI.NavigationViewItemHeader();
                    header.Content = "Ghenterprise";
                    _navigationView.MenuItems.Add(header);
                }
                _navigationView.MenuItems.Add(navItem);
            }
        }

        private async void OnLoaded()
        {
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName);
                return;
            }
            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageKey = item.GetValue(NavHelper.NavigateToProperty) as string;
            NavigationService.Navigate(pageKey);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsView))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType);
            var pageKey = menuItem.GetValue(NavHelper.NavigateToProperty) as string;
            return pageKey == navigatedPageKey;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }
    }
}
