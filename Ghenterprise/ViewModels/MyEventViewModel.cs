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
    public class MyEventViewModel : ViewModelBase
    {
        public MyEventViewModel()
        {

        }

        private Event _selected;
        private bool _isEnabled = true;
        public Event Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { Set(ref _isEnabled, value); }
        }
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _addNewEventCommand;
        public ICommand AddNewEventCommand => _addNewEventCommand ?? (_addNewEventCommand = new RelayCommand(new Action(OnNewClick)));
        private ICommand _editEventCommand;
        public ICommand EditEventCommand => _editEventCommand ?? (_editEventCommand = new RelayCommand(new Action(OnEditClick)));
        private ICommand _deleteEventCommand;
        public ICommand DeleteEventCommand => _deleteEventCommand ?? (_deleteEventCommand = new RelayCommand(new Action(OnDeleteClick)));

        public ObservableCollection<Event> Source { get; private set; } = new ObservableCollection<Event>();
        private EventService eventService = new EventService();


        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            var items = await eventService.GetEventsOfOwner();
            items.ForEach(item => { Source.Add(item); });

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = Source.FirstOrDefault();
            }
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(EventCreateViewModel).FullName);
        }

        private void OnEditClick()
        {
            if (Selected != null)
            {
                NavigationService.Navigate(typeof(EventCreateViewModel).FullName, Selected.Id);
            }
        }


        private async void OnDeleteClick()
        {
            if (Selected != null)
            {
                IsEnabled = true;
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
                        var success = await eventService.DeleteEvent(Selected.Id);
                        if (success)
                        {
                            ContentDialog successDialog = new ContentDialog();
                            successDialog.Title = "event verwijderd.";
                            successDialog.PrimaryButtonText = "ok";
                            await successDialog.ShowAsync();

                            Source.Clear();
                            var items = await eventService.GetEventsOfOwner();
                            items.ForEach(item => { Source.Add(item); });
                            if (Source.Count > 0)
                                Selected = Source.First();
                        }
                        else
                        {
                            ContentDialog failureDialog = new ContentDialog();
                            failureDialog.Title = "event niet verwijderd.";
                            failureDialog.PrimaryButtonText = "ok";
                            await failureDialog.ShowAsync();
                        }
                    }
                }
                catch (Exception)
                {
                    ContentDialog exceptionDialog = new ContentDialog();
                    exceptionDialog.Title = "Er ging iets mis probeer later opnieuw.";
                    exceptionDialog.PrimaryButtonText = "ok";
                    await exceptionDialog.ShowAsync();
                }
                IsEnabled = true;
            }
        }
    }
}
