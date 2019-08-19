using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Ghenterprise.Views;
using Ghenterprise.Views.Overview;

namespace Ghenterprise.ViewModels
{
    class ViewModelLocator
    {
        public const string OverviewPageKey = "Overview";


        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            var navigation = new NavigationService();
            navigation.Configure(OverviewPageKey, typeof(Overview));

            SimpleIoc.Default.Register<INavigationService>(() => navigation);
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<SkeletonViewModel>();
            SimpleIoc.Default.Register<OverviewViewModel>();
        }

        public UserViewModel User {
            get
            {
                return new UserViewModel();
            }
        }

        public SkeletonViewModel Skeleton
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SkeletonViewModel>();
            }
        }

        public OverviewViewModel Overview
        {
            get
            {
                return ServiceLocator.Current.GetInstance<OverviewViewModel>();
            }
        }
    }
}
