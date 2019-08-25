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

        public PromotionService promoService { get; set; }


        public async Task LoadDataAsync()
        {
            Source.Clear();
            List<Promotion> promoList = await promoService.GetPromosAsync();
            promoList.ForEach((item) => Source.Add(item));
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
