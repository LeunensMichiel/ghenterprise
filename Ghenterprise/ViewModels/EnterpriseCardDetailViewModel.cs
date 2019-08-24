using GalaSoft.MvvmLight;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.ViewModels
{
    public class EnterpriseCardDetailViewModel : ViewModelBase
    {
        private Enterprise _enterprise;

        public Enterprise Enterprise
        {
            get { return _enterprise; }
            set { Set(ref _enterprise, value); }
        }

        public EnterpriseCardDetailViewModel()
        {

        }

        public async Task InitializeAsync(long Id)
        {
            //var data = await SampleDataService.GetContentGridDataAsync();
            //Item = data.First(i => i.OrderID == orderID);
        }
    }
}
