using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class EventViewModel: ViewModelBase
    {
        public EventViewModel()
        {
            eventService = new EventService();
        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Event>(OnItemClick));

        public ObservableCollection<Event> Source { get; } = new ObservableCollection<Event>();

        public EventService eventService { get; set; }


        public async Task LoadDataAsync()
        {
            Source.Clear();

            List<Event> eventList = await eventService.GetEventsAsync();
            eventList.ForEach((item) => Source.Add(item));
        }

        private void OnItemClick(Event clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(EventCardDetailViewModel).FullName, clickedItem.Id);
            }
        }
    }
}
