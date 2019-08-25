using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Ghenterprise.Data;
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
        private ICommand _editPromoCommand;
        public ICommand EditPromoCommand => _editPromoCommand ?? (_editPromoCommand = new RelayCommand(new Action(OnEditClick)));
        private ICommand _deletePromoCommand;
        public ICommand DeletePromoCommand => _deletePromoCommand ?? (_deletePromoCommand = new RelayCommand(new Action(OnDeleteClick)));

        public ObservableCollection<Promotion> Source { get; private set; } = new ObservableCollection<Promotion>();
        private PromotionService promotionService = new PromotionService();

        public async Task LoadDataAsync(MasterDetailsViewState viewState)
        {
            Source.Clear();

            var items = await promotionService.GetPromosAsync();
            System.Diagnostics.Debug.WriteLine(items.Count());
            items.ForEach(item => { Source.Add(item); });

            if (viewState == MasterDetailsViewState.Both)
            {
                Selected = Source.FirstOrDefault();
            }
        }

        private void OnNewClick()
        {
            NavigationService.Navigate(typeof(PromotionCreateViewModel).FullName);
        }

        private void OnEditClick()
        {
            throw new NotImplementedException();
        }


        private void OnDeleteClick()
        {
            throw new NotImplementedException();
        }
    }
}
