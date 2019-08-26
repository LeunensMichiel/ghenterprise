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

namespace Ghenterprise.Views.Enterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EnterpriseCreateView : Page
    {
        private EnterpriseCreateViewModel ViewModel
        {
            get { return ViewModelLocator.Current.EnterpriseCreate; }
        }

        public EnterpriseCreateView()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string Id)
            {
                await ViewModel.LoadDataAsync(Id);
            } else
            {
                //List<TimePicker> TimePickers = new List<TimePicker>();

                //TimePickers.Add(MoStartTime);
                //TimePickers.Add(MoEndTime);

                //TimePickers.Add(TuStartTime);
                //TimePickers.Add(TuEndTime);

                //TimePickers.Add(WeStartTime);
                //TimePickers.Add(WeEndTime);

                //TimePickers.Add(ThStartTime);
                //TimePickers.Add(ThEndTime);

                //TimePickers.Add(FrStartTime);
                //TimePickers.Add(FrEndTime);

                //TimePickers.Add(SaStartTime);
                //TimePickers.Add(SaEndTime);

                //TimePickers.Add(SuStartTime);
                //TimePickers.Add(SuEndTime);

                //ViewModel.initTimePickers(TimePickers);
                await ViewModel.LoadDataAsync();
            }
        }
    }
}
