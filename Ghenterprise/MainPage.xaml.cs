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
using System.Windows;
using Windows.UI.Popups;
using Ghenterprise.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Ghenterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        TestData data;
        public UserViewModel User { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            //TestText.DataContext = "wauw
            //User = new UserViewModel(new Models.User());
            System.Diagnostics.Debug.WriteLine("CHECK");
            //textBox.DataContext = data;
        }

        public class TestData
        {
            public String Test { get; set; }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //BindingExpression expression = textBox.GetBindingExpression(TextBox.TextProperty);
            var dialog = new MessageDialog("Before updateSource, test = " + data.Test);
            await dialog.ShowAsync();
            //expression.UpdateSource();
            dialog = new MessageDialog("After updateSource, test = " + data.Test);
            await dialog.ShowAsync();
        }
    }
}
