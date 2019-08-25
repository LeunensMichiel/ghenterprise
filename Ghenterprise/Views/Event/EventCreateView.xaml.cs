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


namespace Ghenterprise.Views.Event
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventCreateView : Page
    {
        private EventCreateViewModel EventCreateViewModel
        {
            get { return ViewModelLocator.Current.EventCreate; }
        }

        public EventCreateView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await EventCreateViewModel.LoadDataAsync();
            StartDate.MinDate = DateTimeOffset.Now;
            EndDate.MinDate = DateTimeOffset.Now;
        }
    }
}
