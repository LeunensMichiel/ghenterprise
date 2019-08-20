using System;
using Ghenterprise.Services;
using Ghenterprise.Views.Skeleton;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;


namespace Ghenterprise
{
    sealed partial class App : Application
    {

        // Meer info over de ActivationService Protocol in UWP https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/activation.md (@Nico dit zijn de Guidelines van Microsoft)
        // > The ActivationService is in charge of handling the applications initialization and activation. -> Handig voor later Notificaties en Authenticatie toe te voegen

        private Lazy<ActivationService> _activationService;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }


        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!e.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(e);
            }
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(ViewModels.OverviewViewModel), new Lazy<UIElement>(CreateSkelleton));
        }

        private UIElement CreateSkelleton()
        {
            return new SkeletonView();
        }
    }
}
