using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Ghenterprise.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        private UserViewModel UserViewModel => ViewModelLocator.Current.User;
        private ToastService toastService = new ToastService();
        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get
            {
                return _errorVisibility;
            }
            set
            {
                _errorVisibility = value;
                RaisePropertyChanged("ErrorVisibility");
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                Set(ref _isEnabled, value);
            }
        }

        private ICommand _loginCommand;
        public ICommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(new Action(LoginUser)));

        private User _user;
        public User User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

        private string _errorText;
        public string ErrorText
        {
            get { return _errorText; }
            set { Set(ref _errorText, value); }
        }

        private async void LoginUser()
        {
            ErrorVisibility = Visibility.Collapsed;
            if (string.IsNullOrEmpty(User.Email))
            {
                ErrorText = "Email is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(User.Password))
            {
                ErrorText = "Wachtwoord is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            IsEnabled = false;
            try
            {
                await UserViewModel.LogUserIn(User);
                if (UserViewModel.IsLoggedIn)
                {
                    User = UserViewModel.User;
                    IsEnabled = true;
                    NavigationService.Navigate(typeof(OverviewViewModel).FullName);
                } else
                {
                    ErrorText = "Wachtwoord en Email komen niet overeen!";
                    ErrorVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowToast("Inloggen mislukt!", ex.Message);
            }
            IsEnabled = true;
        }

        private ICommand _registrationCommand;
        public ICommand RegistrationCommand => _registrationCommand ?? (_registrationCommand = new RelayCommand(new Action(ToRegistration)));

        private void ToRegistration()
        {
            NavigationService.Navigate(typeof(RegistrationViewModel).FullName);
        }

        public LoginViewModel()
        {
            User = new User();
        }
    }
}
