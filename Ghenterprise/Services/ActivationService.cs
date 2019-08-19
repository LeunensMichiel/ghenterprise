using Ghenterprise.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Ghenterprise.Services
{
    internal class ActivationService
    {
        private readonly App _app;
        private readonly Type _defaultNavItem;
        private Lazy<UIElement> _skeleton;

        private object _lastActivationArgs;


        public ActivationService(App app, Type defaultNavItem, Lazy<UIElement> skeleton = null)
        {
            _app = app;
            _skeleton = skeleton;
            _defaultNavItem = defaultNavItem;
        }

        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                //  Hier Services initializeren die nodig zijn voor de app activation (zoals login enzo)
                // > Nog in splashscreen

                await InitializeAsync();
                // TODO: Login Systeem

                if (Window.Current.Content == null)
                {
                    // Extra check om niet alles opnieuw te doen moest de app al actief zijn
                    Window.Current.Content = _skeleton?.Value ?? new Frame();
                }
            }

            // Depending on activationArgs one of ActivationHandlers or DefaultActivationHandler
            // will navigate to the first page
            await HandleActivationAsync(activationArgs);
            _lastActivationArgs = activationArgs;

            if (IsInteractive(activationArgs))
            {
                // Ensure the current window is active
                Window.Current.Activate();

                // Tasks after activation
                await StartupAsync();
            }
        }

        private bool IsInteractive(object args)
        {
            return args is IActivatedEventArgs;
        }

        private async Task HandleActivationAsync(object activationArgs)
        {
           /* var activationHandler = GetActivationHandlers()
                                                .FirstOrDefault(h => h.CanHandle(activationArgs));*/

        /*    if (activationHandler != null)
            {
                await activationHandler.HandleAsync(activationArgs);
            }*/

            if (IsInteractive(activationArgs))
            {
                var defaultHandler = new DefaultActivationHandler(_defaultNavItem);
                if (defaultHandler.CanHandleActivation(activationArgs))
                {
                    await defaultHandler.HandleActivationAsync(activationArgs);
                }
            }
        }

        private async Task StartupAsync()
        {
            // Bijvoorbeeld de Theme aanpassen met de themeselector
        }

        private async Task InitializeAsync()
        {
            // Bijvoorbeeld de themeselectorservice init
        }

     /*   private IEnumerable<ActivationHandler> GetActivationHandlers()
        {
            yield return Singleton<ToastNotificationsService>.Instance;
        }*/
    }
}
