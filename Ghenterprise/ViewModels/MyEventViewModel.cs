using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class MyEventViewModel : ViewModelBase
    {
        public MyEventViewModel()
        {

        }

        private Event _selected;
        public Event Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _addNewEventCommand;
        public ICommand AddNewEventCommand => _addNewEventCommand ?? (_addNewEventCommand = new RelayCommand(new Action(OnNewClick)));
        private ICommand _editEventCommand;
        public ICommand EditEventCommand => _editEventCommand ?? (_editEventCommand = new RelayCommand(new Action(OnEditClick)));
        private ICommand _deleteEventCommand;
        public ICommand DeleteEventCommand => _deleteEventCommand ?? (_deleteEventCommand = new RelayCommand(new Action(OnDeleteClick)));

        public ObservableCollection<Event> Source { get; private set; } = new ObservableCollection<Event>();
        private EventService eventService = new EventService();


        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            var items = await eventService.GetEventsAsync();
            items.ForEach(item => { Source.Add(item); });

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = Source.FirstOrDefault();
            }
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(EventCreateViewModel).FullName);
        }

        private void OnEditClick()
        {
            throw new NotImplementedException();
        }


        private void OnDeleteClick()
        {
            throw new NotImplementedException();
        }
    }
}
