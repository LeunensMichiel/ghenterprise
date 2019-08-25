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
using Ghenterprise.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ghenterprise.Views.Enterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyEnterpriseDetailView : UserControl
    {
        public static readonly DependencyProperty MasterEnterpriseItemProperty = DependencyProperty.Register("MasterEnterpriseItem", typeof(Models.Enterprise), typeof(MyEnterpriseDetailView), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));
        private string _category = "";

        public Models.Enterprise MasterEnterpriseItem
        {
            get { return GetValue(MasterEnterpriseItemProperty) as Models.Enterprise; }
            set { SetValue(MasterEnterpriseItemProperty, value); }
        }

        public MyEnterpriseDetailView()
        {
            this.InitializeComponent();
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MyEnterpriseDetailView;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }

    }
}
