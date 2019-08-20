using Ghenterprise.Services;
using Ghenterprise.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Ghenterprise.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly string _navElement;

        public NavigationService NavigationService => ViewModelLocator.Current.NavigationServ;

        protected override async Task HandleIntervalAsync(IActivatedEventArgs args)
        {
            object arguments = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }

            NavigationService.Navigate(_navElement, arguments);
            await Task.CompletedTask;
        }

        public DefaultActivationHandler(Type navElement)
        {
            _navElement = navElement.FullName;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // Activatiehandlers hebben gefaald
            return NavigationService.Frame.Content == null && _navElement != null;
        }
    }
}
