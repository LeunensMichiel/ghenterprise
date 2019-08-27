using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Helpers;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Ghenterprise.Views.Settings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace Ghenterprise.ViewModels
{
    public class SkeletonViewModel : ViewModelBase
    {
        //TODO: User Authenticatie toevoegen

        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
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
        public static NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        public WinUI.NavigationViewItem Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _loginCommand;
        private ICommand _itemInvokedCommand;
        private RelayCommand _userProfileCommand;
        private bool _isBusy;

        public UserViewModel UserViewModel
        {
            get { return ViewModelLocator.Current.User; }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                UserProfileCommand.RaiseCanExecuteChanged();
            }
        }


        public SkeletonViewModel()
        {
        }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(OnLogin));

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));
        public RelayCommand UserProfileCommand => _userProfileCommand ?? (_userProfileCommand = new RelayCommand(OnUserProfile, () => !IsBusy));


        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            _keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigatedFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;

            MenuItems = GetMenuItems().ToArray();
            foreach (WinUI.NavigationViewItem navItem in MenuItems)
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

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            UserViewModel.IsLoggedIn = true;
            //IsAuthorized = IsLoggedIn && IdentityService.IsAuthorized();
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            UserViewModel.IsLoggedIn = false;
            UserViewModel.IsAuthorized = false;
            CleanRestrictedPagesFromNavigationHistory();
            GoBackToLastUnrestrictedPage();
        }

        private void CleanRestrictedPagesFromNavigationHistory()
        {
            NavigationService.Frame.BackStack
            .Where(b => Attribute.IsDefined(b.SourcePageType, typeof(Restricted)))
            .ToList()
            .ForEach(page => NavigationService.Frame.BackStack.Remove(page));
        }

        private void GoBackToLastUnrestrictedPage()
        {
            var currentPage = NavigationService.Frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                NavigationService.GoBack();
            }
        }

        private async void OnUserProfile()
        {
            if (UserViewModel.IsLoggedIn)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName);
            }
            else
            {
                IsBusy = true;
                //var loginResult = await IdentityService.LoginAsync();
                //if (loginResult != LoginResultType.Success)
                //{
                //    await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                //    IsBusy = false;
                //}
            }
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

        private void OnLogin()
        {
            NavigationService.Navigate(typeof(LoginViewModel).FullName);
            return;
        }
    }
}
