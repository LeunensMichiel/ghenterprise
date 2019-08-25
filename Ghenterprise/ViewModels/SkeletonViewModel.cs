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
        public RelayCommand UserProfileCommand => _userProfileCommand ?? (_userProfileCommand = new RelayCommand(OnUserProfile, () => !IsBusy));
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
        private RelayCommand _userProfileCommand;
        private UserService userService = new UserService();
        private bool _isBusy;
        private bool _popupIsOpen = false;
        private string _username = "Inloggen";
        private ContentDialog dialog;
        private bool _isRegsitrating = false;
        private Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                UserProfileCommand.RaiseCanExecuteChanged();
            }
        }

        public string Username {
            get
            {
                return _username;
            }
            set
            {
                Set(ref _username, value);
            }
        }

        public static NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        public SkeletonViewModel()
        {
            if(localSettings.Values["Username"] != null)
            {
                Username = localSettings.Values["Username"] as string;
            }
        }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { Set(ref _isBackEnabled, value); }
        }

        public bool PopupIsOpen {
            get
            {
                return _popupIsOpen;
            }
            set
            {
                Set(ref _popupIsOpen, value);
            }
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

        private async void OnUserProfile()
        {
            //Models.User user = new Models.User();
            //if (localSettings.Values["Username"] == null)
            //{
            //    try
            //    {
            //        string error = "";
            //        do
            //        {
            //            error = null;

            //            if (_isRegsitrating && (
            //                user.Password == "Wachtwoorden komen niet overeen" 
            //                || user.Password == "Wachtwoord is leeg" 
            //                || user.Password == "Email is leeg of verkeerd formaat"
            //                || user.Password == "Familienaam is leeg"
            //                || user.Password == "Voornaam is leeg"))
            //            {
            //                error = user.Password == ""?null:user.Password;
            //            }

            //            if (user.Password == "Password invalid" || user.Password == "User doesn't exist")
            //            {
            //                error = "Verkeerde email/wachtwoord combinatie";
            //            }

            //            user = await LoginDialog(error);
            //            Debug.WriteLine(user.Token);
            //        }
            //        while ((user.Password != "Password valid" && user.Password != "stop") || (user.Token != "email doesn't exists"));

            //        if (user.Password == "Password valid")
            //        {
            //            ContentDialog dialog = new ContentDialog();
            //            dialog.Title = "Inloggen";
            //            dialog.Content = "Gebruiker ingelogd!";
            //            dialog.PrimaryButtonText = "Wow! Dankuwel!";
            //            await dialog.ShowAsync();
            //            localSettings.Values["Token"] = user.Token;
            //            localSettings.Values["Username"] = user.Email;
            //            Username = user.Email;
            //        }

                    

            //        if (user.Token == "email doesn't exists")
            //        {
            //            user = await userService.PostRegisterUser(user);
            //            dialog.Title = "Account aanmaken";
            //            dialog.Content = "Account aangemaakt!";
            //            dialog.PrimaryButtonText = "Wow! Dankuwel!";
            //            await dialog.ShowAsync();
            //            localSettings.Values["Token"] = user.Token;
            //            localSettings.Values["Username"] = user.Email;
            //            Username = user.Email;
            //        }

            //    }
            //    catch (Exception)
            //    {
            //        ContentDialog dialog = new ContentDialog();
            //        dialog.Title = "Er ging iets verkeerd";
            //        dialog.Content = "Inloggen is gefaald";
            //        dialog.IsSecondaryButtonEnabled = true;
            //        dialog.PrimaryButtonText = "Ok";
            //        dialog.SecondaryButtonText = "Niet ok";
            //        await dialog.ShowAsync();
            //    }
            //}
            //else
            //{
            //    localSettings.Values["Username"] = null;
            //    localSettings.Values["Token"] = null;
            //    Username = "Inloggen";
            //}
           
            
        }

        private async Task<Models.User> LoginDialog(string error)
        {
            TextBox emailTextBox = new TextBox();
            emailTextBox.AcceptsReturn = false;
            emailTextBox.Height = 32;
            emailTextBox.PlaceholderText = "Email";
            emailTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            PasswordBox passwordTextBox = new PasswordBox();
            passwordTextBox.Height = 32;
            passwordTextBox.PlaceholderText = "Wachtwoord";
            passwordTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            Button registerButton = new Button();
            registerButton.Content = _isRegsitrating?"Inloggen":"Registreren";
            registerButton.Click += on_registration;

            StackPanel panel = new StackPanel();

            TextBox firstnameTextBox = new TextBox();

            TextBox lastnameTextBox = new TextBox();

            PasswordBox verifyPasswordTextBox = new PasswordBox();

            if (error != null)
            {
                TextBlock errorBlock = new TextBlock();
                errorBlock.Height = 32;
                errorBlock.Text = error;
                errorBlock.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);
                errorBlock.Foreground = new SolidColorBrush(Colors.Red);
                panel.Children.Add(errorBlock);
            }

            if (_isRegsitrating)
            {
                firstnameTextBox.AcceptsReturn = false;
                firstnameTextBox.Height = 32;
                firstnameTextBox.PlaceholderText = "Voornaam";
                firstnameTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

                lastnameTextBox.AcceptsReturn = false;
                lastnameTextBox.Height = 32;
                lastnameTextBox.PlaceholderText = "Achternaam";
                lastnameTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

                verifyPasswordTextBox.Height = 32;
                verifyPasswordTextBox.PlaceholderText = "Verifiëer wachtwoord";
                verifyPasswordTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

                panel.Children.Add(firstnameTextBox);
                panel.Children.Add(lastnameTextBox);
            }


            panel.Children.Add(emailTextBox);
            panel.Children.Add(passwordTextBox);

            if (_isRegsitrating)
            {
                panel.Children.Add(verifyPasswordTextBox);
            }

            panel.Children.Add(registerButton);

            dialog = new ContentDialog();
            dialog.Content = panel;
            dialog.Title = _isRegsitrating ? "Account aanmaken" : "Inloggen";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = _isRegsitrating?"Registreren" :"Aanmelden";
            dialog.SecondaryButtonText = "Annuleren";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                if (_isRegsitrating)
                {
                    if (firstnameTextBox.Text.Trim() == "")
                    {
                        return new Models.User
                        {
                            Password = "Voornaam is leeg"
                        };
                    }

                    if (lastnameTextBox.Text.Trim() == "")
                    {
                        return new Models.User
                        {
                            Password = "Familienaam is leeg"
                        };
                    }

                    if (emailTextBox.Text.Trim() == "" || !emailRegex.Match(emailTextBox.Text.Trim()).Success)
                    {
                        return new Models.User
                        {
                            Password = "Email is leeg of verkeerd formaat"
                        };
                    }

                    if (passwordTextBox.Password == "")
                    {
                        return new Models.User
                        {
                            Password = "Wachtwoord is leeg"
                        };
                    }

                    if (passwordTextBox.Password != verifyPasswordTextBox.Password)
                    {
                        return new Models.User
                        {
                            Password = "Wachtwoorden komen niet overeen"
                        };
                    }

                    return await userService.GetCheckEmail(new Models.User
                    {
                        Firstname = firstnameTextBox.Text,
                        Lastname = lastnameTextBox.Text,
                        Email =  emailTextBox.Text,
                        Password = passwordTextBox.Password
                    });
                } else
                {
                    return await userService.PostLogin(new Models.User
                    {
                        Email = emailTextBox.Text,
                        Password = passwordTextBox.Password
                    });
                }
            }
            else
            {
                return new Models.User
                {
                    Password = "stop"
                };
            } 
        }

        private void on_registration(object sender, RoutedEventArgs e)
        {
            _isRegsitrating = !_isRegsitrating;
            dialog?.Hide();
            OnUserProfile();
        }

        private async Task<Models.User> RegisterDialog()
        {
            dialog?.Hide();

            TextBox firstnameTextBox = new TextBox();
            firstnameTextBox.AcceptsReturn = false;
            firstnameTextBox.Height = 32;
            firstnameTextBox.PlaceholderText = "Firstname";
            firstnameTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            TextBox lastnameTextBox = new TextBox();
            lastnameTextBox.AcceptsReturn = false;
            lastnameTextBox.Height = 32;
            lastnameTextBox.PlaceholderText = "Lastname";
            lastnameTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            TextBox emailTextBox = new TextBox();
            emailTextBox.AcceptsReturn = false;
            emailTextBox.Height = 32;
            emailTextBox.PlaceholderText = "Email";
            emailTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            PasswordBox passwordTextBox = new PasswordBox();
            passwordTextBox.Height = 32;
            passwordTextBox.PlaceholderText = "Wachtwoord";
            passwordTextBox.Margin = new Windows.UI.Xaml.Thickness(10, 10, 10, 10);

            StackPanel panel = new StackPanel();


            panel.Children.Add(firstnameTextBox);
            panel.Children.Add(lastnameTextBox);
            panel.Children.Add(emailTextBox);
            panel.Children.Add(passwordTextBox);

            dialog = new ContentDialog();
            dialog.Content = panel;
            dialog.Title = "Inloggen";
            dialog.IsSecondaryButtonEnabled = true;
            dialog.PrimaryButtonText = "Aanmelden";
            dialog.SecondaryButtonText = "Annuleren";
            if (await dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return await userService.PostRegisterUser(new Models.User
                {
                    Email = firstnameTextBox.Text,
                    Password = passwordTextBox.Password
                });
            }
            else
            {
                return new Models.User
                {
                    Password = "stop"
                };
            }

        }
    }
}
