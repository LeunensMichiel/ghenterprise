using Ghenterprise.ViewModels;
using System;

using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ghenterprise.Views.Overview
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Overview : Page
    {
        PivotItem pivot = null;

        private OverviewViewModel OverviewViewModel
        {
            get { return ViewModelLocator.Current.Overview; }
        }

        private MapViewModel MapViewModel
        {
            get { return ViewModelLocator.Current.Map; }
        }

        public Overview()
        {
            InitializeComponent();
        }

        private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pivot = (PivotItem)(sender as Pivot).SelectedItem;

            switch (pivot.Header.ToString())
            {
                case "Abonnementen":
                    MapViewModel.Cleanup();
                    await OverviewViewModel.LoadDataAsync();
                    break;
                case "Alle":
                    MapViewModel.Cleanup();
                    await OverviewViewModel.LoadDataAsync();
                    break;
                case "Kaart":
                    await MapViewModel.InitializeAsync(mapControl);
                    break;
            }
        }
    }
}
