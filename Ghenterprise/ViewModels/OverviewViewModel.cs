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

        private EnterpriseService entService = new EnterpriseService();
        private CategoryService catService = new CategoryService();
        private ToastService toastService = new ToastService();

        private List<string> _catListNames = new List<string>();
        private List<Enterprise> _entlist = new List<Enterprise>();
        private List<Enterprise> _subscriptionlist = new List<Enterprise>();
        private string _selectedCatName = "";
        private string _searchQuery = "";
        private bool _isEnabled = true;

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Enterprise>(OnItemClick));

        public ObservableCollection<Enterprise> Source { get; } = new ObservableCollection<Enterprise>();
        public ObservableCollection<Enterprise> SubscriptionSource { get; } = new ObservableCollection<Enterprise>();

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

        public string SeachQuery {
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
            IsEnbabled = false;
            try
            {
                SubscriptionSource.Clear();

               var items = await entService.GetSubscriptionsAsync();
               items.ForEach((item) => SubscriptionSource.Add(item));
                System.Diagnostics.Debug.WriteLine(SubscriptionSource.Count());

                if (SubscriptionSource.Count() > 0)
                {
                    IsDataUnavailable = false;
                }

            }
            catch (Exception)
            {
                toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
            }
            IsEnbabled = true;
        }

        private void FilterSource()
        {
            Source.Clear();
            SubscriptionSource.Clear();
            List<Enterprise> filteredList = _entlist;
            List<Enterprise> filteredSubscriptions = _subscriptionlist;
            if (_selectedCatName != "Alle" || _searchQuery.Trim() != "")
            {
                if (_selectedCatName != "Alle")
                {
                    filteredList = filteredList.Where((e) => e.Categories.Select(c => c.Name).Contains(_selectedCatName)).ToList();
                    filteredSubscriptions = filteredSubscriptions.Where((e) => e.Categories.Select(c => c.Name).Contains(_selectedCatName)).ToList();
                }

                if (_searchQuery.Trim() != "")
                {
                    filteredList = filteredList.Where((e) => e.Name.ToLower().Contains(_searchQuery.ToLower())).ToList();
                    filteredSubscriptions = filteredSubscriptions.Where((e) => e.Name.ToLower().Contains(_searchQuery.ToLower())).ToList();

                }
            } 

            filteredList.ForEach(f => Source.Add(f));
            filteredSubscriptions.ForEach(f => SubscriptionSource.Add(f));
        }

        private void OnItemClick(Enterprise clickedItem)
        {
            if (clickedItem != null)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                NavigationService.Navigate(typeof(EnterpriseCardDetailViewModel).FullName, clickedItem.Id);
            }
        }
    }

}
