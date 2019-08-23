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
using Ghenterprise.Views.Enterprise;
using Ghenterprise.Views.Event;

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
            SimpleIoc.Default.Register<MapViewModel>();
            SimpleIoc.Default.Register<EnterpriseViewModel>();

            
            //  Viewmodels met View
            SimpleIoc.Default.Register<OverviewViewModel>();
            NavigationServ.Configure(typeof(OverviewViewModel).FullName, typeof(Overview));
            SimpleIoc.Default.Register<EventViewModel>();
            NavigationServ.Configure(typeof(EventViewModel).FullName, typeof(EventView));
            SimpleIoc.Default.Register<PromotionViewModel>();
            NavigationServ.Configure(typeof(PromotionViewModel).FullName, typeof(PromotionView));

            SimpleIoc.Default.Register<MyEnterpriseViewModel>();
            NavigationServ.Configure(typeof(MyEnterpriseViewModel).FullName, typeof(MyEnterpriseView));
            SimpleIoc.Default.Register<MyEventViewModel>();
            NavigationServ.Configure(typeof(MyEventViewModel).FullName, typeof(MyEventsView));
            SimpleIoc.Default.Register<MyPromotionsViewModel>();
            NavigationServ.Configure(typeof(MyPromotionsViewModel).FullName, typeof(MyPromotionsView));

            SimpleIoc.Default.Register<SettingsViewModel>();
            NavigationServ.Configure(typeof(SettingsViewModel).FullName, typeof(SettingsView));

            //Create en Edit ViewModels
            SimpleIoc.Default.Register<EnterpriseCreateViewModel>();
            NavigationServ.Configure(typeof(EnterpriseCreateViewModel).FullName, typeof(EnterpriseCreateView));

            SimpleIoc.Default.Register<EventCreateViewModel>();
            NavigationServ.Configure(typeof(EventCreateViewModel).FullName, typeof(EventCreateView));
        }

        public NavigationService NavigationServ => SimpleIoc.Default.GetInstance<NavigationService>();

        public UserViewModel User => SimpleIoc.Default.GetInstance<UserViewModel>();

        public EnterpriseViewModel Enterprise => SimpleIoc.Default.GetInstance<EnterpriseViewModel>();

        public SkeletonViewModel Skeleton => SimpleIoc.Default.GetInstance<SkeletonViewModel>();

        public OverviewViewModel Overview => SimpleIoc.Default.GetInstance<OverviewViewModel>();

        public EventViewModel Event => SimpleIoc.Default.GetInstance<EventViewModel>();

        public PromotionViewModel Promotion => SimpleIoc.Default.GetInstance<PromotionViewModel>();

        public MyEnterpriseViewModel MyEnterprise => SimpleIoc.Default.GetInstance<MyEnterpriseViewModel>();

        public MyEventViewModel MyEvents => SimpleIoc.Default.GetInstance<MyEventViewModel>();

        public MyPromotionsViewModel MyPromotions => SimpleIoc.Default.GetInstance<MyPromotionsViewModel>();

        public SettingsViewModel Settings => SimpleIoc.Default.GetInstance<SettingsViewModel>();

        public MapViewModel Map => SimpleIoc.Default.GetInstance<MapViewModel>();

        public EnterpriseCreateViewModel EnterpriseCreate => SimpleIoc.Default.GetInstance<EnterpriseCreateViewModel>();

        public EventCreateViewModel EventCreate => SimpleIoc.Default.GetInstance<EventCreateViewModel>();

    }
}
