using GalaSoft.MvvmLight;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.ViewModels
{
    public class EventCardDetailViewModel : ViewModelBase
    {
        private Event _event;

        public Event Event
        {
            get { return _event; }
            set { Set(ref _event, value); }
        }

        public EventCardDetailViewModel()
        {

        }

        public async Task InitializeAsync(long orderID)
        {
            //var data = await SampleDataService.GetContentGridDataAsync();
            //Item = data.First(i => i.OrderID == orderID);
        }
    }
}
