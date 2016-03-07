using DataServices;
using Prism.Unity.Windows;
using System;
using System.Threading.Tasks;
using ThinkWiseMan.Helpers;
using ToastTileCreator;
using Windows.ApplicationModel.Activation;
using Windows.Storage;

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
            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            RegisterTypeIfMissing(typeof(IBackgroundTaskManager), typeof(BackgroundTaskManager), false);
            RegisterTypeIfMissing(typeof(IXmlDataService), typeof(XmlDataService), false);
            RegisterTypeIfMissing(typeof(IToastManager), typeof(ToastManager), false);
        }
    }
}
