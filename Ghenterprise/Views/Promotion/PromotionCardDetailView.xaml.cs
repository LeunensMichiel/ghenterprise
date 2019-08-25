using Ghenterprise.Services;
using Ghenterprise.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
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
    public sealed partial class PromotionCardDetailView : Page
    {
        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;
        private PromotionCardDetailViewModel ViewModel
        {
            get { return ViewModelLocator.Current.PromoDetail; }
        }

        public PromotionCardDetailView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string Id)
            {
                await ViewModel.InitializeAsync(Id);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (e.NavigationMode == NavigationMode.Back)
            {
                NavigationService.Frame.SetListDataItemForNextConnectedAnimation(ViewModel.Promotion);
            }
        }
    }
}
