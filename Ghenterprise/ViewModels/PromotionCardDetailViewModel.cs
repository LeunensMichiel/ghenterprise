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

namespace Ghenterprise.ViewModels
{
    public class PromotionCardDetailViewModel : ViewModelBase
    {
        private Promotion _promotion;
        public Promotion Promotion
        {
            get { return _promotion; }
            set { Set(ref _promotion, value); }
        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        public PromotionService promotionService { get; set; }
        private ICommand _clickEnterpriseCommand;
        public ICommand EnterpriseClickCommand => _clickEnterpriseCommand ?? (_clickEnterpriseCommand = new RelayCommand(new Action(OnItemClick)));

        public PromotionCardDetailViewModel()
        {
            promotionService = new PromotionService();
        }

        public async Task InitializeAsync(string Id)
        {
            Promotion = await promotionService.GetPromoAsync(Id);
        }

        private void OnItemClick()
        {
            NavigationService.Navigate(typeof(EnterpriseCardDetailViewModel).FullName, Promotion.Enterprise.Id);
        }
    }
}
