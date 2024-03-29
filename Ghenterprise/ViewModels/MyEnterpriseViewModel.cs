﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace Ghenterprise.ViewModels
{
    public class MyEnterpriseViewModel : ViewModelBase
    {
        private Enterprise _selected;
        private EnterpriseService entService = new EnterpriseService();
        private List<Enterprise> _entlist = new List<Enterprise>();
        private bool _isEnabled = true;
        private ToastService toastService = new ToastService();
        public Enterprise Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { Set(ref _isEnabled, value); }
        }
        public ObservableCollection<Enterprise> Source { get; private set; } = new ObservableCollection<Enterprise>();
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _addNewEnterpriseCommand;
        public ICommand AddNewEnterpriseCommand => _addNewEnterpriseCommand ?? (_addNewEnterpriseCommand = new RelayCommand(new Action(OnNewClick)));
        private ICommand _editEnterpriseCommand;
        public ICommand EditEnterpriseCommand => _editEnterpriseCommand ?? (_editEnterpriseCommand = new RelayCommand(new Action(OnEditClick)));
        private ICommand _deleteEnterpriseCommand;
        public ICommand DeleteEnterpriseCommand => _deleteEnterpriseCommand ?? (_deleteEnterpriseCommand = new RelayCommand(new Action(OnDeleteClick)));

        public MyEnterpriseViewModel()
        {

        }

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            try
            {
                Source.Clear();

                _entlist = await entService.GetEnterprisesByOwner();
                _entlist.ForEach(ent => { Source.Add(ent); });


                if (viewState == MasterDetailsViewState.Both)
                {
                    if (Source.Count > 0)
                        Selected = Source.First();
                }
            } catch (Exception)
            {
                toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
            }
            
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(EnterpriseCreateViewModel).FullName);
        }

        private void OnEditClick()
        {
            NavigationService.Navigate(typeof(EnterpriseCreateViewModel).FullName, Selected.Id);
        }


        private async void OnDeleteClick()
        {
            IsEnabled = false;
            try
            {
                ContentDialog dialog = new ContentDialog();
                dialog.Title = $"Bent u zeker dat u {Selected.Name} wilt verwijderen?";
                dialog.IsSecondaryButtonEnabled = true;
                dialog.PrimaryButtonText = "Ja";
                dialog.SecondaryButtonText = "Nee";
                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    Debug.WriteLine(Selected.Id);
                    var success = await entService.DeleteEnterprise(Selected.Id);
                    if (success)
                    {
                        toastService.ShowToast("Onderneming verwijderd", "");

                        Source.Clear();
                        _entlist = await entService.GetEnterprisesByOwner();
                        _entlist.ForEach(ent => { Source.Add(ent); });
                        if (Source.Count > 0)
                            Selected = Source.First();
                    }
                    else
                    {
                        toastService.ShowToast("Onderneming niet verwijderd", "probeer later opnieuw");
                    }
                }
            } catch(Exception)
            {
                toastService.ShowToast("Er ging iets mis", "probeer later opnieuw");
            }
            IsEnabled = true;

        }

    }
}
