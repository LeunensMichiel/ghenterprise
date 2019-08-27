using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        public UserViewModel UserViewModel => ViewModelLocator.Current.User;

        private ICommand _logoutCommand;
        public ICommand LogOutCommand => _logoutCommand ?? (_logoutCommand = new RelayCommand(OnLogout));

        private void OnLogout()
        {
            UserViewModel.LogUserOut();
            NavigationService.Navigate(typeof(OverviewViewModel).FullName);
        }

        public SettingsViewModel()
        {

        }
    }
}
