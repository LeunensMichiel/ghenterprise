using GalaSoft.MvvmLight;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public PromotionCardDetailViewModel()
        {

        }

        public async Task InitializeAsync(long Id)
        {
            //var data = await SampleDataService.GetContentGridDataAsync();
            //Item = data.First(i => i.OrderID == orderID);
        }
    }
}
