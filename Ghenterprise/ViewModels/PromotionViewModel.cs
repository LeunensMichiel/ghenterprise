using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Models;
using Ghenterprise.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class PromotionViewModel : ViewModelBase
    {
        public PromotionViewModel()
        { }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _itemClickCommand;

        public ICommand ItemClickCommand => _itemClickCommand ?? (_itemClickCommand = new RelayCommand<Promotion>(OnItemClick));

        public ObservableCollection<Promotion> Source { get; } = new ObservableCollection<Promotion>();


        public async Task LoadDataAsync()
        {
            Source.Clear();

            Promotion promotion = new Promotion();
            promotion.Description = "Lorem Ipsum Doloret";
            promotion.Name = "Solden Leunes Media";
            promotion.StartDate = new DateTime().Date;
            promotion.EndDate = new DateTime().AddDays(7).Date;

            Promotion promotion2 = new Promotion();
            promotion2.Description = "Lorem Ipsum Doloret";
            promotion2.Name = "Solden Leunes Media";
            promotion2.StartDate = new DateTime().Date;
            promotion2.EndDate = new DateTime().AddDays(7).Date;

            Source.Add(promotion);
            Source.Add(promotion2);
            //// TODO WTS: Replace this with your actual data
            //var data = await SampleDataService.GetContentGridDataAsync();
            //foreach (var item in data)
            //{
            //    Source.Add(item);
            //}

        }

        private void OnItemClick(Promotion clickedItem)
        {
            if (clickedItem != null)
            {
                //NavigationService.Frame.SetListDataItemForNextConnectedAnimation(clickedItem);
                //NavigationService.Navigate(typeof(ContentGridDetailViewModel).FullName, clickedItem.OrderID);
            }
        }
    }
}
