using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Ghenterprise.ViewModels
{
    class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<UserViewModel>();
            SimpleIoc.Default.Register<EnterpriseViewModel>();
        }

        public UserViewModel User {
            get
            {
                return new UserViewModel();
            }
        }

        public EnterpriseViewModel Enterprise {
            get
            {
                return new EnterpriseViewModel();
            }
        }
    }
}
