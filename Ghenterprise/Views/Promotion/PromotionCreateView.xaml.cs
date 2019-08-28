using Ghenterprise.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ghenterprise.Views.Promotion
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PromotionCreateView : Page
    {
        private PromotionCreateViewModel PromoCreateViewModel
        {
            get { return ViewModelLocator.Current.PromotionCreate; }
        }

        public PromotionCreateView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string id)
            {
                await PromoCreateViewModel.LoadDataAsync(id);
            } else
            {
                PromoCreateViewModel._isEditScreen = false;
                await PromoCreateViewModel.LoadDataAsync();
            }
            
            StartDate.MinDate = DateTimeOffset.Now;
            EndDate.MinDate = DateTimeOffset.Now;
        }
    }
}
