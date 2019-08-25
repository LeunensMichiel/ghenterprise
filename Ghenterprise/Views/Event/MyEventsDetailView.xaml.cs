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

namespace Ghenterprise.Views.Event
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyEventsDetailView : Page
    {
        public MyEventsDetailView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty MasterEventItemProperty = DependencyProperty.Register("MasterEventItem", typeof(Models.Event), typeof(MyEventsDetailView), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));


        public Models.Event MasterEventItem
        {
            get { return GetValue(MasterEventItemProperty) as Models.Event; }
            set { SetValue(MasterEventItemProperty, value); }
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MyEventsDetailView;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
