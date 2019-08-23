using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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

        public ObservableCollection<Event> Source { get; private set; } = new ObservableCollection<Event>();

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            Event eventie = new Event();
            eventie.Description = "Kijk dit bedrijf doet een event wajow";
            eventie.Name = "Afterparty";
            eventie.StartDate = new DateTime().Date;
            eventie.EndDate = new DateTime().AddDays(7).Date;

            Event eventie2 = new Event();
            eventie2.Description = "Heeee wij ook haha LOL!";
            eventie2.Name = "Pukkelpop";
            eventie2.StartDate = new DateTime().Date;
            eventie2.EndDate = new DateTime().AddDays(7).Date;

            Source.Add(eventie);
            Source.Add(eventie2);

            /*var data = await SampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }*/

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = Source.First();
            }
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(EventCreateViewModel).FullName);
        }
    }
}
