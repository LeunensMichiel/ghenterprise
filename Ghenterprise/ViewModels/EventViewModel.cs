﻿using GalaSoft.MvvmLight;
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

        private ToastService toastService = new ToastService();
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Event>(OnItemClick));

        public ObservableCollection<Event> Source { get; } = new ObservableCollection<Event>();
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

        public EventService eventService { get; set; }


        public async Task LoadDataAsync()
        {
            try
            {
                Source.Clear();

                var eventList = await eventService.GetEventsAsync();
                eventList.ForEach((item) => Source.Add(item));

                if (Source.Count() > 0)
                {
                    IsDataUnavailable = false;
                }
            } catch(Exception)
            {
                toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
            }
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
