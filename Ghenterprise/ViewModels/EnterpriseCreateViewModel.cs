﻿using GalaSoft.MvvmLight;
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
        private ToastService toastService = new ToastService();

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
        public bool _isEditScreen = false;

        public List<Opening_Hours> OpeningHours { get; set; } = new List<Opening_Hours>();
        private Opening_Hours _monday = new Opening_Hours()
        {
            Day_Of_Week = 0,
        };
        public Opening_Hours Monday
        {
            get { return _monday; }
            set
            {
                Set(ref _monday, value);
            }
        }
        private Opening_Hours _tuesday = new Opening_Hours()
        {
            Day_Of_Week = 1
        };
        public Opening_Hours Tuesday
        {
            get { return _tuesday; }
            set
            {
                Set(ref _tuesday, value);
            }
        }
        private Opening_Hours _wednesday = new Opening_Hours()
        {
            Day_Of_Week = 2
        };
        public Opening_Hours Wednesday
        {
            get { return _wednesday; }
            set
            {
                Set(ref _wednesday, value);
            }
        }
        private Opening_Hours _thursday = new Opening_Hours()
        {
            Day_Of_Week = 3
        };
        public Opening_Hours Thursday
        {
            get { return _thursday; }
            set
            {
                Set(ref _thursday, value);

            }
        }
        private Opening_Hours _friday = new Opening_Hours()
        {
            Day_Of_Week = 4
        };
        public Opening_Hours Friday
        {
            get { return _friday; }
            set
            {
                Set(ref _friday, value);

            }
        }
        private Opening_Hours _saturday = new Opening_Hours()
        {
            Day_Of_Week = 5
        };
        public Opening_Hours Saturday
        {
            get { return _saturday; }
            set
            {
                Set(ref _saturday, value);

            }
        }
        private Opening_Hours _sunday = new Opening_Hours()
        {
            Day_Of_Week = 6
        };
        public Opening_Hours Sunday
        {
            get { return _sunday; }
            set
            {
                Set(ref _sunday, value);

            }
        }


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

        public Visibility ErrorVsibility
        {
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

        public string ErrorText
        {
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

        public bool IsEnabled
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
            }
        }


        public string TagList
        {
            get
            {
                return _tagList;
            }
            set
            {
                _tagList = value;
                Enterprise.Tags = value.Split(",").Select((t) => new Tag { Name = t }).ToList();
                Enterprise = Enterprise;
                RaisePropertyChanged("TagList");
            }
        }

        private ICommand _cancelClickCommand;
        private ICommand _saveClickCommand;
        public ICommand CancelClickCommand => _cancelClickCommand ?? (_cancelClickCommand = new RelayCommand(new Action(OnCancelClick)));
        public ICommand SaveClickCommand => _saveClickCommand ?? (_saveClickCommand = new RelayCommand(new Action(OnSaveClick)));

        public EnterpriseCreateViewModel()
        {
        }

        public async Task LoadDataAsync(string enterprise_id = null)
        {
            IsEnabled = false;
            IsEnabled = IsEnabled;
            _catList = await catService.GetAllCategoriesAsync();
            CategoryNames = _catList.Select((c) => c.Name).ToList();
            if (enterprise_id != null)
            {
                _isEditScreen = true;
                try
                {
                    var items = await entService.GetEnterpriseAsync(enterprise_id);
                    Enterprise = items.First();
                    if (Enterprise.Tags != null)
                    {
                        if (Enterprise.Tags.Count > 0)
                        {
                            TagList = string.Join(",", Enterprise.Tags.Select(t => t.Name));
                        }
                    }
                    if (Enterprise.Categories != null)
                    {
                        if (Enterprise.Categories.Count > 0)
                        {
                            SelectedCatName = Enterprise.Categories[0].Name;
                        }
                    }
                    if (Enterprise.Opening_Hours != null)
                    {
                        if (Enterprise.Opening_Hours.Count > 0)
                        {
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 0).FirstOrDefault() != null)
                            {
                                Monday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 0).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 1).FirstOrDefault() != null)
                            {
                                Tuesday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 1).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 2).FirstOrDefault() != null)
                            {
                                Wednesday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 2).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 3).FirstOrDefault() != null)
                            {
                                Thursday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 3).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 4).FirstOrDefault() != null)
                            {
                                Friday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 4).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 5).FirstOrDefault() != null)
                            {
                                Saturday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 5).First();
                            }
                            if (Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 6).FirstOrDefault() != null)
                            {
                                Sunday = Enterprise.Opening_Hours.Where(o => o.Day_Of_Week == 6).First();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
                    IsEnabled = true;
                    IsEnabled = IsEnabled;
                }
            }

            OpeningHours.Add(Monday);
            OpeningHours.Add(Tuesday);
            OpeningHours.Add(Wednesday);
            OpeningHours.Add(Thursday);
            OpeningHours.Add(Friday);
            OpeningHours.Add(Saturday);
            OpeningHours.Add(Sunday);

            IsEnabled = true;
            IsEnabled = IsEnabled;
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

            List<Opening_Hours> temp = new List<Opening_Hours>();
            foreach (Opening_Hours day in OpeningHours)
            {
                if (day.Start != day.End)
                {
                    temp.Add(day);
                }
            }
            Enterprise.Opening_Hours = temp;

            Enterprise.Location.City = new City
            {
                Id = "PFTDg1mGC2rs"
            };
            Enterprise.Categories.Clear();
            Enterprise.Categories.Add(_catList.Where((c) => c.Name == SelectedCatName).FirstOrDefault());
            IsEnabled = false;
            try
            {
                if (_isEditScreen)
                {
                    result = await entService.UpdateEnterprise(Enterprise);
                }
                else
                {
                    result = await entService.SaveEnterprise(Enterprise);
                }
            }
            catch (Exception ex)
            {
                ErrorText = "Er ging iets fout. Onderneming is niet opgeslagen.";
                ErrorVsibility = Visibility.Visible;
            }

            if (result)
            {
                NavigationService.GoBack();
            }
            else
            {
                ErrorText = "Onderneming is niet opgeslagen.";
                ErrorVsibility = Visibility.Visible;
            }
            IsEnabled = true;
        }
    }
}
