using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public EnterpriseService entService { get; set; }

        public OverviewViewModel()
        {
            entService = new EnterpriseService();
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();

            List<Enterprise> entList = await entService.GetEnterprisesAsync();

            entList.ForEach((item) => Source.Add(item));

            Debug.WriteLine(JsonConvert.SerializeObject(entList));

            Enterprise enti = new Enterprise();
            enti.Name = "Leunes Media";
            enti.Description = "Fotografie / Webdev";
            enti.Location = new Location
            {
                Street_Number = 7,
                Street = new Street
                {
                    Id= "A9wOjHPQxnoG"
                },
                City = new City
                {
                    Id= "PFTDg1mGC2rs"
                }
            };

            bool success = await entService.SaveEnterprise(enti);
            Debug.WriteLine(success);
            //Enterprise enti2 = new Enterprise();
            //enti2.Name = "Kastart";
            //enti2.Description = "Fuck ik heb honger";
            //enti2.Id = "QWERTY";
            //enti2.DateCreated = new DateTime();
            //Source.Add(enti2);
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
