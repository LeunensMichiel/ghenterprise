using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class EventViewModel: ViewModelBase
    {
        public EventViewModel()
        {

        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Event>(OnItemClick));

        public ObservableCollection<Event> Source { get; } = new ObservableCollection<Event>();

        public async Task LoadDataAsync()
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
            //// TODO WTS: Replace this with your actual data
            //var data = await SampleDataService.GetContentGridDataAsync();
            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}

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
