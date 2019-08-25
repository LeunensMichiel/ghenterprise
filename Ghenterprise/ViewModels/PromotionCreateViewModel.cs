using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace Ghenterprise.ViewModels
{
    public class PromotionCreateViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        private EnterpriseService entService = new EnterpriseService();
        private PromotionService promotionService = new PromotionService();
        public ObservableCollection<Enterprise> Enterprises { get; set; } = new ObservableCollection<Enterprise>();

        private Enterprise _enterprise = new Enterprise();
        public Enterprise Enterprise
        {
            get { return _enterprise; }
            set
            {
                if (_enterprise != value)
                {
                    _enterprise = value;
                    RaisePropertyChanged("Enterprise");
                }
            }
        }

        private Promotion _promotion = new Promotion();
        public Promotion Promotion
        {
            get { return _promotion; }
            set
            {
                _promotion = value;
                RaisePropertyChanged("Promotion");
            }
        }

        private ICommand _cancelClickCommand;
        public ICommand CancelClickCommand => _cancelClickCommand ?? (_cancelClickCommand = new RelayCommand(new Action(OnCancelClick)));
        private ICommand _saveClickCommand;
        public ICommand SaveClickCommand => _saveClickCommand ?? (_saveClickCommand = new RelayCommand(new Action(OnSaveClick)));

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsReadOnly");
            }
        }
        private string _errorText = "";
        private Visibility _errorVisibility = Visibility.Collapsed;
        public Visibility ErrorVisibility
        {
            get => _errorVisibility;
            set
            {
                _errorVisibility = value;
                RaisePropertyChanged("ErrorVisibility");
            }
        }
        public string ErrorText
        {
            get => _errorText;
            set
            {
                _errorText = value;
                RaisePropertyChanged("ErrorText");
            }
        }

        public PromotionCreateViewModel()
        {

        }

        public async Task LoadDataAsync()
        {
            Enterprises.Clear();

            var items = await entService.GetEnterprisesAsync();
            items.ForEach((item) => Enterprises.Add(item));

        }

        private async void OnSaveClick()
        {
            bool result = false;
            ErrorVisibility = Visibility.Collapsed;
            if (Promotion.Name == null || Promotion.Name.Trim() == "")
            {
                ErrorText = "Naam is niet ingevuld";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            if (Promotion.Description == null || Promotion.Description.Trim() == "")
            {
                ErrorText = "Beschrijving is niet ingevuld";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            if (Promotion.Start_Date == null || Promotion.End_Date == null || Promotion.End_Date < Promotion.Start_Date)
            {
                ErrorText = "Datums kloppen niet";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            if (Enterprise == null)
            {
                ErrorText = "Ghenterprise is niet ingevuld";
                ErrorVisibility = Visibility.Visible;
                return;
            }

            IsEnabled = false;
            Promotion.Enterprise = Enterprise;
            result = await promotionService.SavePromoAsync(Promotion);
            if (result)
            {
                NavigationService.GoBack();
            }
            else
            {
                ErrorText = "Er ging iets mis. Promotie is niet opgeslagen";
                ErrorVisibility = Visibility.Visible;
            }
            IsEnabled = true;
        }

        private void OnCancelClick()
        {
            NavigationService.GoBack();

        }
    }
}
