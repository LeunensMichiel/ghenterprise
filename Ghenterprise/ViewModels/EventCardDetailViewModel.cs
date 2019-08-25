using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class EventCardDetailViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        public EventService eventService { get; set; }
        private ICommand _clickEnterpriseCommand;
        public ICommand EnterpriseClickCommand => _clickEnterpriseCommand ?? (_clickEnterpriseCommand = new RelayCommand(new Action(OnItemClick)));

        private Event _event;
        public Event Event
        {
            get { return _event; }
            set { Set(ref _event, value); }
        }

        public EventCardDetailViewModel()
        {
            eventService = new EventService();
        }

        public async Task InitializeAsync(string Id)
        {
            Event = await eventService.GetEventAsync(Id);
        }

        private void OnItemClick()
        {
            NavigationService.Navigate(typeof(EnterpriseCardDetailViewModel).FullName, Event.Enterprise.Id);
        }
    }
}
