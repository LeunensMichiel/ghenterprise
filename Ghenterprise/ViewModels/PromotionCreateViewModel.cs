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
    public class PromotionCreateViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _cancelClickCommand;
        public ICommand CancelClickCommand => _cancelClickCommand ?? (_cancelClickCommand = new RelayCommand(new Action(OnCancelClick)));

        public PromotionCreateViewModel()
        {

        }

        private void OnCancelClick()
        {
            NavigationService.GoBack();

        }
    }
}
