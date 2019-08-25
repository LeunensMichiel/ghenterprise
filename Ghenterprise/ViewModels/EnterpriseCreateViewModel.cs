using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Ghenterprise.ViewModels
{
    public class EnterpriseCreateViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        private CategoryService catService = new CategoryService();
        private EnterpriseService entService = new EnterpriseService();

        private Enterprise _enterprise = new Enterprise();
        private List<Category> _catList = new List<Category>();
        private List<string> _catListNames = new List<string>();
        private Category _category = new Category();
        private Location _location = new Location();
        private string _selectedCatName = "";
        private string _errorText = "";
        private Visibility _errorVisibility = Visibility.Collapsed;
        private bool _isEnabled = true;
        private string _tagList = "";

        public Enterprise Enterprise
        {
            get
            {
                return _enterprise;
            }
            set
            {
                _enterprise = value;
                RaisePropertyChanged("Enterprise");
            }
        }

        public Visibility ErrorVsibility {
            get
            {
                return _errorVisibility;
            }
            set
            {
                _errorVisibility = value;
                RaisePropertyChanged("ErrorVsibility");
            }
        }

        public string ErrorText {
            get
            {
                return _errorText;
            }
            set
            {
                _errorText = value;
                RaisePropertyChanged("ErrorText");
            }
        }

        public bool IsEnabled {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                RaisePropertyChanged("IsReadOnly");
            }
        }


        public List<string> CategoryNames {
            get
            {
                return _catListNames;
            }
            set
            {
                Set(ref _catListNames, value);
            }
        }

        public string SelectedCatName {
            get
            {
                return _selectedCatName;
            }
            set
            {
                Set(ref _selectedCatName, value);
            }
        }

        public Category SelectedCategory
        {
            get
            {
                return _category;
            }
            set
            {
                Set(ref _category, value);
            }
        }

        public string TagList {
            get
            {
                return _tagList;
            }
            set
            {
                _tagList = value;
                Enterprise.Tags = value.Split(",").Select((t) => new Tag { Name = t }).ToList();
                Enterprise.Tags.ForEach((t) => Debug.WriteLine(t.Name));
                Enterprise = Enterprise;
                RaisePropertyChanged("TagList");
            }
        }

        public Location Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                RaisePropertyChanged("Location");
            }
        }

        private ICommand _cancelClickCommand;
        private ICommand _saveClickCommand;
        public ICommand CancelClickCommand => _cancelClickCommand ?? (_cancelClickCommand = new RelayCommand(new Action(OnCancelClick)));
        public ICommand SaveClickCommand => _saveClickCommand ?? (_saveClickCommand = new RelayCommand(new Action(OnSaveClick)));

        public EnterpriseCreateViewModel()
        {
            LoadDataAsync();
        }

        public async void LoadDataAsync()
        {
            _catList = await catService.GetAllCategoriesAsync();
            CategoryNames = _catList.Select((c) => c.Name).ToList();
        }

        private void OnCancelClick()
        {
            NavigationService.GoBack();
        }

        private async void OnSaveClick()
        {
            bool result = false;

            ErrorVsibility = Visibility.Collapsed;
            if (Enterprise.Name == null || Enterprise.Name.Trim() == "")
            {
                ErrorText = "Naam is niet ingevuld";
                ErrorVsibility = Visibility.Visible;
                return;
            }

            if (Enterprise.Description == null || Enterprise.Description.Trim() == "")
            {
                ErrorText = "Descriptie is niet ingevuld";
                ErrorVsibility = Visibility.Visible;
                return;
            }

            if (Enterprise.Location.Street.Name == null || Enterprise.Location.Street.Name == "")
            {
                ErrorText = "Straatnaam is niet ingevuld";
                ErrorVsibility = Visibility.Visible;
                return;
            }

            Enterprise.Location.City = new City
            {
                Id = "PFTDg1mGC2rs"
            };
            Enterprise.Categories.Clear();
            Debug.WriteLine(SelectedCatName);
            Enterprise.Categories.Add(_catList.Where((c) => c.Name == SelectedCatName).FirstOrDefault());
            Debug.WriteLine(JsonConvert.SerializeObject(Enterprise));
            IsEnabled = false;
            result = await entService.SaveEnterprise(Enterprise);
            if (result)
            {
                NavigationService.GoBack();
            } else
            {
                ErrorText = "Onderneming is niet opgeslagen";
                ErrorVsibility = Visibility.Visible;
            }
            IsEnabled = true;
        }
    }
}
