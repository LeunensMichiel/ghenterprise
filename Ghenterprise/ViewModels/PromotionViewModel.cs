using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class PromotionViewModel : ViewModelBase
    {
        public PromotionViewModel()
        {
            promoService = new PromotionService();
        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Promotion>(OnItemClick));

        public ObservableCollection<Promotion> Source { get; } = new ObservableCollection<Promotion>();

        private bool _isDataUnavailable = true;
        public bool IsDataUnavailable
        {
            get => _isDataUnavailable;
            set
            {
                _isDataUnavailable = value;
                    RaisePropertyChanged("IsDataUnavailable");
            }
        }

        public PromotionService promoService { get; set; }


        public async Task LoadDataAsync()
        {
            Source.Clear();
            var items = await promoService.GetPromosAsync();
            items.ForEach((item) => Source.Add(item));
            if (Source.Count() > 0)
            {
                IsDataUnavailable = false;
            }
        }

        private void OnItemClick(Promotion clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(PromotionCardDetailViewModel).FullName, clickedItem.Id);
            }
        }
    }
}
