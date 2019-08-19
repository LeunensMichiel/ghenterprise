using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Ghenterprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Ghenterprise.ViewModels
{
    public class SkeletonViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private readonly NavigationItem OverviewItem = new NavigationItem(0xF246, "Overzicht", "Overview");

        private object _selectedNavItem;
        public object SelectedNavItem
        {
            get => _selectedNavItem;
            set => Set(ref _selectedNavItem, value);
        }

        private bool _isPaneOpen = true;
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => Set(ref _isPaneOpen, value);
        }

        private IEnumerable<NavigationItem> _items;
        public IEnumerable<NavigationItem> Items
        {
            get => _items;
            set => Set(ref _items, value);
        }

        public RelayCommand NavigateCommand { get; private set; }

        private IEnumerable<NavigationItem> GetItems()
        {
            yield return OverviewItem;
        }

        public SkeletonViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            NavigateCommand = new RelayCommand(NavigateCommandAction);
            Items = GetItems().ToArray();
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("Overview");
        }

        public async void NavigateTo(string key)
        {
            Console.WriteLine(key);
            switch (key)
            {
                case "Overview":
                    _navigationService.NavigateTo(key);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
