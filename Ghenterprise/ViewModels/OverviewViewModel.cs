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
        private List<string> _catListNames = new List<string>();
        private List<Enterprise> _entlist = new List<Enterprise>();
        private string _selectedCatName = "";
        private string _searchQuery = "";

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Enterprise>(OnItemClick));

        public ObservableCollection<Enterprise> Source { get; } = new ObservableCollection<Enterprise>();


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
            Source.Clear();

            _entlist = await entService.GetEnterprisesAsync();

            _entlist.ForEach((item) => Source.Add(item));

            List<Category> catList = await catService.GetAllCategoriesAsync();
            CategoryNames.Add("Categorie");
            CategoryNames.AddRange( catList.Select((c) => c.Name).ToList());

        }

        private void FilterSource()
        {
            Source.Clear();
            List<Enterprise> filteredList = _entlist;
            if (_selectedCatName != "Categorie" || _searchQuery.Trim() != "")
            {
                if (_selectedCatName != "Categorie")
                {
                    filteredList = filteredList.Where((e) => e.Categories.Select(c => c.Name).Contains(_selectedCatName)).ToList();
                }

                if (_searchQuery.Trim() != "")
                {
                    filteredList = filteredList.Where((e) => e.Name.ToLower().Contains(_searchQuery.ToLower())).ToList();

                }
            } 

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
    }

}
