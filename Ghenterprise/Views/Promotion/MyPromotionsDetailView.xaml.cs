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
    public sealed partial class MyPromotionsDetailView : Page
    {
        public MyPromotionsDetailView()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty MasterPromoItemProperty = DependencyProperty.Register("MasterPromoItem", typeof(Models.Promotion), typeof(MyPromotionsDetailView), new PropertyMetadata(null, OnMasterMenuItemPropertyChanged));


        public Models.Promotion MasterPromoItem
        {
            get { return GetValue(MasterPromoItemProperty) as Models.Promotion; }
            set { SetValue(MasterPromoItemProperty, value); }
        }

        private static void OnMasterMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as MyPromotionsDetailView;
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
