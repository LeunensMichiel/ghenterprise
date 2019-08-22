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

    public class OverviewViewModel : ViewModelBase
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Enterprise>(OnItemClick));

        public ObservableCollection<Enterprise> Source { get; } = new ObservableCollection<Enterprise>();


        public async Task LoadDataAsync()
        {
            Source.Clear();

            Enterprise enti = new Enterprise();
            enti.Name = "Leunes Media";
            enti.Description = "Fotografie / Webdev";
            enti.Id = "AFER";
            enti.DateCreated = new DateTime();
            Source.Add(enti);

            Enterprise enti2 = new Enterprise();
            enti2.Name = "Kastart";
            enti2.Description = "Fuck ik heb honger";
            enti2.Id = "QWERTY";
            enti2.DateCreated = new DateTime();
            Source.Add(enti2);
            //// TODO WTS: Replace this with your actual data
            //var data = await SampleDataService.GetContentGridDataAsync();
            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}

        }

        private void OnItemClick(Enterprise clickedItem)
        {
            if (clickedItem != null)
            {
                //NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                //NavigationService.Navigate(typeof(ContentGridDetailViewModel).FullName, clickedItem.OrderID);
            }
        }
    }

}
