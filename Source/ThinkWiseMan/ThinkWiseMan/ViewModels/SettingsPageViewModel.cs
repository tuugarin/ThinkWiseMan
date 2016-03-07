using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Prism.Windows.Navigation;
using Windows.UI.Core;
using ThinkWiseMan.Helpers;
using System.Diagnostics;
using ToastTileCreator;
using Windows.UI.Notifications;

namespace ThinkWiseMan.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        IBackgroundTaskManager backgroundTaskManager;
        IToastManager toastManager;
        public SettingsPageViewModel(IBackgroundTaskManager _backgroundTaskManager, IToastManager _toastManager)
        {
            _notificationSheduleTime = (TimeSpan)(localSettings.Values["NotifcationSheduleTime"]);
            _notificationEnabled = (bool)(localSettings.Values["NotificationEnabled"]);
            backgroundTaskManager = _backgroundTaskManager;
            toastManager = _toastManager;
        }

        private bool _notificationEnabled;
        public bool NotificationEnabled
        {
            get
            {
                return _notificationEnabled;

            }
            set
            {
                _notificationEnabled = value;
                localSettings.Values["NotificationEnabled"] = _notificationEnabled;
                ChangeNotificationEnabled();
                OnPropertyChanged("NotificationEnabled");
            }
        }

        private TimeSpan _notificationSheduleTime;
        public TimeSpan NotifcationSheduleTime
        {
            get { return _notificationSheduleTime; }
            set
            {
                _notificationSheduleTime = value;
                localSettings.Values["NotifcationSheduleTime"] = _notificationSheduleTime;
                ChangeNotifcationSheduleTime();
            }
        }

        async void ChangeNotifcationSheduleTime()
        {
            await toastManager.ClearAllSheduledNotificationsAsync();
            if(DateTime.Now.Hour == _notificationSheduleTime.Hours && DateTime.Now.Minute == _notificationSheduleTime.Minutes)
            {
                var toast = await toastManager.CreateToastNotificationAsync(DateTime.Now.Day, DateTime.Now.Month);
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.Show(toast);
            }
            else
            {
                var timeToStart = _notificationSheduleTime;
                if (DateTime.Now.TimeOfDay > _notificationSheduleTime)
                {
                    timeToStart = timeToStart.Add(new TimeSpan(24,00,00));
                }
                var totalMinutes = Math.Abs((DateTime.Now.TimeOfDay.Subtract(timeToStart)).TotalMinutes);
                var toast = await toastManager.CreateSheduledToastNotificationAsync(DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.AddMinutes(totalMinutes));
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                toastNotifier.AddToSchedule(toast);

            }

            await backgroundTaskManager.UnregisterNotificationTaskAsync();
            await backgroundTaskManager.RegisterNotificationTaskAsync();
        }
        async void ChangeNotificationEnabled()
        {
            if (_notificationEnabled == false)
            {
                await toastManager.ClearAllSheduledNotificationsAsync();
                await backgroundTaskManager.UnregisterNotificationTaskAsync();
            }
            else
            {
                 ChangeNotifcationSheduleTime();
            }

        }
        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            // Show UI in title bar if opted-in and in-app backstack is not empty.
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            base.OnNavigatedTo(e, viewModelState);
        }
    }
}
