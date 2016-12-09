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
                TimeSpan time = new TimeSpan(22, 30, 00);
                localSettings.Values["NotifcationSheduleTime"] = time;
            }

            //if (args.Kind == ActivationKind.ToastNotification)
            //    NavigationService.Navigate("Main", args.Arguments);
            //else 

           var result = NavigationService.Navigate("Main", null);
       
            return Task.FromResult<object>(null);
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {

            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                var frame = await InitializeFrameAsync(args);
               // frame.
               // frame.Navigate(typeof(MainPage),null);

                await OnLaunchApplicationAsync(null);
               // NavigationService.Navigate("Main", null);
            }
            //base.OnActivated(args);
            // await InitializeFrameAsync(args);
            //else
            //{
            //    if (args.Kind == ActivationKind.ToastNotification)
            //    {
            //        var toastActivationArgs = args as ToastNotificationActivatedEventArgs;

            //        NavigationService.Navigate("Main", toastActivationArgs.Argument);
            //    }
            //    else NavigationService.Navigate("Main", null);
            //}

            base.OnActivated(args);

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
