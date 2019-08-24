using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Models;
using Ghenterprise.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ghenterprise.ViewModels
{
    public class MyPromotionsViewModel : ViewModelBase
    {
        public MyPromotionsViewModel()
        {

        }

        private Promotion _selected;
        public Promotion Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private ICommand _addNewPromotionCommand;
        public ICommand AddNewPromoCommand => _addNewPromotionCommand ?? (_addNewPromotionCommand = new RelayCommand(new Action(OnNewClick)));

        public ObservableCollection<Promotion> Source { get; private set; } = new ObservableCollection<Promotion>();

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            Promotion promotion = new Promotion();
            promotion.Description = "Lorem Ipsum Doloret";
            promotion.Name = "Solden Leunes Media";
            promotion.StartDate = new DateTime().Date;
            promotion.EndDate = new DateTime().AddDays(7).Date;

            Promotion promotion2 = new Promotion();
            promotion2.Description = "Lorem Ipsum Doloret";
            promotion2.Name = "Solden Nicolaas zijn Lekkere spaghetti!!!!";
            promotion2.StartDate = new DateTime().Date;
            promotion2.EndDate = new DateTime().AddDays(7).Date;

            Source.Add(promotion);
            Source.Add(promotion2);

            /*var data = await SampleDataService.GetMasterDetailDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }*/

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = Source.First();
            }
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(PromotionCreateViewModel).FullName);
        }
    }
}
