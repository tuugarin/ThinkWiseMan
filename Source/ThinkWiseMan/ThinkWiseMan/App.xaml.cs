using DataServices;
using Microsoft.EntityFrameworkCore;
using Prism.Unity.Windows;
using System;
using System.Threading.Tasks;
using ThinkWiseMan.Helpers;
using ThinkWiseMan.Views;
using ToastTileCreator;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ThinkWiseMan
{
    sealed partial class App : PrismUnityApplication
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values["NotificationEnabled"] == null)
                localSettings.Values["NotificationEnabled"] = true;

            if (localSettings.Values["NotifcationSheduleTime"] == null)
            {
                TimeSpan time = new TimeSpan(08, 00, 00);
                localSettings.Values["NotifcationSheduleTime"] = time;
            }

            NavigationService.Navigate("Main", null);

            return Task.FromResult<object>(null);
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            base.OnActivated(args);

            if (args.Kind == ActivationKind.ToastNotification)
            {
                var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

                NavigationService.Navigate("Main", toastActivationArgs.Argument);
            }
            else NavigationService.Navigate("Main", null);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            RegisterTypeIfMissing(typeof(IBackgroundTaskManager), typeof(BackgroundTaskManager), false);
            RegisterTypeIfMissing(typeof(IXmlDataService), typeof(XmlDataService), false);
            RegisterTypeIfMissing(typeof(IToastManager), typeof(ToastManager), false);
            RegisterTypeIfMissing(typeof(ISqlDataService), typeof(SqlDataService), false);
        }

    }
}
