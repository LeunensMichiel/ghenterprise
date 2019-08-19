using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using Ghenterprise.Services;
using Ghenterprise.Views;
using Ghenterprise.Views.Overview;

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
/*            NavigationServ.Configure(typeof(EnterpriseViewModel).FullName, typeof(Enter));
*/            SimpleIoc.Default.Register<OverviewViewModel>();
            NavigationServ.Configure(typeof(OverviewViewModel).FullName, typeof(Overview));
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

        public NavigationService NavigationServ => SimpleIoc.Default.GetInstance<NavigationService>();
    }
}
