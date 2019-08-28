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
    public class RegistrationViewModel : ViewModelBase
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

        private ICommand _registerCommand;
        public ICommand RegisterCommand => _registerCommand ?? (_registerCommand = new RelayCommand(new Action(RegisterUser)));

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

        private string _passRepeat;
        public string PasswordRepeat
        {
            get { return _passRepeat; }
            set { Set(ref _passRepeat, value); }
        }

        private async void RegisterUser()
        {
            ErrorVisibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(User.Firstname))
            {
                ErrorText = "Voornaam is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(User.Lastname))
            {
                ErrorText = "Achternaam is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                ErrorText = "Email is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            if (string.IsNullOrEmpty(User.Password) || string.IsNullOrEmpty(PasswordRepeat))
            {
                ErrorText = "Wachtwoord is niet ingevuld!";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            if (!PasswordRepeat.Equals(User.Password))
            {
                ErrorText = "Wachtwoorden komen niet overeen!";
                ErrorVisibility = Visibility.Visible;
                return;
            }
            IsEnabled = false;
            try
            {
                await UserViewModel.Register(User);
                if (UserViewModel.IsLoggedIn)
                {
                    User = UserViewModel.User;
                    IsEnabled = true;
                    NavigationService.Navigate(typeof(OverviewViewModel).FullName);
                }
                else
                {
                    ErrorText = "Email reeds in gebruik!";
                    ErrorVisibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                toastService.ShowToast("Registreren mislukt!", ex.Message);
            }
            IsEnabled = true;
        }

        private ICommand _loginCommand;
        public ICommand ToLoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(new Action(ToLogin)));

        private void ToLogin()
        {
            NavigationService.GoBack();
        }
        public RegistrationViewModel()
        {
            User = new User();
        }
    }
}
