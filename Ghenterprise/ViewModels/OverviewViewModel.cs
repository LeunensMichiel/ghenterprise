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
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{

    public class OverviewViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        public UserViewModel UserViewModel => ViewModelLocator.Current.User;

        private EnterpriseService entService = new EnterpriseService();
        private CategoryService catService = new CategoryService();
        private ToastService toastService = new ToastService();

        private List<string> _catListNames = new List<string>();
        private List<Enterprise> _entlist = new List<Enterprise>();
        private string _selectedCatName = "";
        private string _searchQuery = "";
        private bool _isEnabled = true;

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Enterprise>(OnItemClick));
        private ICommand _eventClickCommand;
        public ICommand EventClickCommand => _eventClickCommand ?? (_eventClickCommand = new RelayCommand<Event>(OnEventClick));
        private ICommand _promoClickCommand;
        public ICommand PromoClickCommand => _promoClickCommand ?? (_promoClickCommand = new RelayCommand<Promotion>(OnPromoClick));
        private ICommand _promosCommand;
        public ICommand PromosCommand => _promosCommand ?? (_promosCommand = new RelayCommand(new Action(GoToPromos)));
        private ICommand _eventsCommand;
        public ICommand EventsCommand => _eventsCommand ?? (_eventsCommand = new RelayCommand(new Action(GoToEvents)));

        public ObservableCollection<Enterprise> Source { get; } = new ObservableCollection<Enterprise>();
        public ObservableCollection<Event> EventsSource { get; } = new ObservableCollection<Event>();
        public ObservableCollection<Promotion> PromoSource { get; } = new ObservableCollection<Promotion>();

        private bool _isDataAvailable = false;
        public bool IsDataAavailable
        {
            get => _isDataAvailable;
            set
            {
                Set(ref _isDataAvailable, value);
            }
        }

        public bool IsEnbabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                Set(ref _isEnabled, value);
            }
        }

        public List<string> CategoryNames
        {
            get
            {
                return _catListNames;
            }
            set
            {
                Set(ref _catListNames, value);
            }
        }
        public string SelectedCatName
        {
            get
            {
                return _selectedCatName;
            }
            set
            {
                Set(ref _selectedCatName, value);
                FilterSource();
            }
        }

        public string SeachQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                Set(ref _searchQuery, value);
                FilterSource();
            }
        }

        public OverviewViewModel()
        {

        }

        public async Task LoadDataAsync()
        {
            IsEnbabled = false;
            try
            {
                Source.Clear();
                _entlist.Clear();
                CategoryNames.Clear();
                SeachQuery = "";
                SelectedCatName = "Alle";

                _entlist = await entService.GetEnterprisesAsync();
                _entlist.ForEach((item) => Source.Add(item));

                List<Category> catList = await catService.GetAllCategoriesAsync();
                CategoryNames.Add("Alle");
                CategoryNames.AddRange(catList.Select((c) => c.Name).ToList());

            }
            catch (Exception)
            {
                toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
            }
            IsEnbabled = true;
        }

        public async Task LoadSubsAsync()
        {
            EventsSource.Clear();
            PromoSource.Clear();

            if (UserViewModel.IsLoggedIn)
            {
                IsEnbabled = false;
                try
                {
                    var events = await entService.GetSubscriptionsEventsAsync();
                    events.ForEach((item) => EventsSource.Add(item));

                    var promos = await entService.GetSubscriptionsPromosAsync();
                    promos.ForEach((promo) => PromoSource.Add(promo));

                    if (EventsSource.Count() > 0 || PromoSource.Count() > 0)
                    {
                        IsDataAavailable = true;
                    }

                }
                catch (Exception)
                {
                    toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
                }
                IsEnbabled = true;
            }
            else
            {
                IsDataAavailable = false;
            }
        }

        private void FilterSource()
        {
            Source.Clear();
            List<Enterprise> filteredList = _entlist;
            if (SelectedCatName != "Alle" || SeachQuery.Trim() != "")
            {
                if (SelectedCatName != "Alle")
                {
                    filteredList = filteredList.Where((e) => e.Categories.Select(c => c.Name).Contains(SelectedCatName)).ToList();
                }

                if (SeachQuery.Trim() != "")
                {
                    filteredList = filteredList.Where((e) => e.Name.ToLower().Contains(SeachQuery.ToLower())).ToList();
                }
            }
            filteredList = filteredList.GroupBy(e => e.Id).Select(en => en.First()).ToList();
            filteredList.ForEach(f => Source.Add(f));
        }

        private void OnItemClick(Enterprise clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(EnterpriseCardDetailViewModel).FullName, clickedItem.Id);
            }
        }
        private void OnEventClick(Event clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(EventCardDetailViewModel).FullName, clickedItem.Id);
            }
        }
        private void OnPromoClick(Promotion clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(PromotionCardDetailViewModel).FullName, clickedItem.Id);
            }
        }

        private void GoToPromos()
        {
            NavigationService.Navigate(typeof(PromotionViewModel).FullName);
        }

        private void GoToEvents()
        {
            NavigationService.Navigate(typeof(EventViewModel).FullName);
        }
    }

}
