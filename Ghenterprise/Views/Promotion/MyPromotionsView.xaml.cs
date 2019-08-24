using Ghenterprise.Services;
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
    public sealed partial class MyPromotionsView : Page
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        private MyPromotionsViewModel MyPromotionsViewModel
        {
            get { return ViewModelLocator.Current.MyPromotions; }
        }

        public MyPromotionsView()
        {
            this.InitializeComponent();
            Loaded += MasterDetailPage_Loaded;
        }

        private async void MasterDetailPage_Loaded(object sender, RoutedEventArgs e)
        {
            await MyPromotionsViewModel.LoadDataAsync(MasterDetailsViewControl.ViewState);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            MyPromotionsViewModel.Selected = null;
        }
    }
}
