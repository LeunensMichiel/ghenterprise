using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Ghenterprise.Services;
using Ghenterprise.Views.Overview;
using Ghenterprise.Views.EventView;
using Ghenterprise.Views.Promotion;
using Ghenterprise.Views.Settings;

namespace Ghenterprise.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        private static ViewModelLocator _current;
        public static ViewModelLocator Current => _current ?? (_current = new ViewModelLocator());

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register(() => new NavigationService());
            SimpleIoc.Default.Register<SkeletonViewModel>();
            SimpleIoc.Default.Register<UserViewModel>();

            
            //  Viewmodels met View
            SimpleIoc.Default.Register<EnterpriseViewModel>();
/*            NavigationServ.Configure(typeof(EnterpriseViewModel).FullName, typeof(Enter)); */
            SimpleIoc.Default.Register<OverviewViewModel>();
            NavigationServ.Configure(typeof(OverviewViewModel).FullName, typeof(Overview));
            SimpleIoc.Default.Register<EventViewModel>();
            NavigationServ.Configure(typeof(EventViewModel).FullName, typeof(EventView));
            SimpleIoc.Default.Register<PromotionViewModel>();
            NavigationServ.Configure(typeof(PromotionViewModel).FullName, typeof(PromotionView));
            SimpleIoc.Default.Register<SettingsViewModel>();
            NavigationServ.Configure(typeof(SettingsViewModel).FullName, typeof(SettingsView));
        }

        public UserViewModel User {
            get
            {
                return SimpleIoc.Default.GetInstance<UserViewModel>();
            }
        }

        public EnterpriseViewModel Enterprise {
            get
            {
                return SimpleIoc.Default.GetInstance<EnterpriseViewModel>();
            }
        }

        public SkeletonViewModel Skeleton
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SkeletonViewModel>();
            }
        }

        public OverviewViewModel Overview
        {
            get
            {
                return SimpleIoc.Default.GetInstance<OverviewViewModel>();
            }
        }

        public EventViewModel Event
        {
            get
            {
                return SimpleIoc.Default.GetInstance<EventViewModel>();
            }
        }

        public PromotionViewModel Promotion
        {
            get
            {
                return SimpleIoc.Default.GetInstance<PromotionViewModel>();
            }
        }

        public NavigationService NavigationServ
        {
            get
            {
                return SimpleIoc.Default.GetInstance<NavigationService>();
            }
        }
        public SettingsViewModel Settings
        {
            get
            {
                return SimpleIoc.Default.GetInstance<SettingsViewModel>();
            }
        }

    }
}
