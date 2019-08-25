using GalaSoft.MvvmLight;
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

        public Enterprise Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
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
            Source.Clear();

            _entlist = await entService.GetEnterprisesAsync();
            _entlist.ForEach(ent => { Source.Add(ent); });


            if (viewState == MasterDetailsViewState.Both)
            {
                if (Source.Count > 0)
                    Selected = Source.First();
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
                        ContentDialog successDialog = new ContentDialog();
                        successDialog.Title = "Onderneming verwijderd.";
                        successDialog.PrimaryButtonText = "ok";
                        await successDialog.ShowAsync();

                        Source.Clear();
                        _entlist = await entService.GetEnterprisesAsync();
                        _entlist.ForEach(ent => { Source.Add(ent); });
                        if (Source.Count > 0)
                            Selected = Source.First();
                    }
                    else
                    {
                        ContentDialog failureDialog = new ContentDialog();
                        failureDialog.Title = "Onderneming niet verwijderd.";
                        failureDialog.PrimaryButtonText = "ok";
                        await failureDialog.ShowAsync();
                    }
                }
            } catch(Exception)
            {
                ContentDialog exceptionDialog = new ContentDialog();
                exceptionDialog.Title = "Er ging iets mis probeer later opnieuw.";
                exceptionDialog.PrimaryButtonText = "ok";
                await exceptionDialog.ShowAsync();
            }
            
        }

    }
}
