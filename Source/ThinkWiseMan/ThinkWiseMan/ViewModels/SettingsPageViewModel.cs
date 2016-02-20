using Prism.Windows.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Prism.Windows.Navigation;
using Windows.UI.Core;

namespace ThinkWiseMan.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public SettingsPageViewModel(INavigationService navigationService)
        {
            _notificationSheduleTime = (TimeSpan)(localSettings.Values["NotifcationSheduleTime"]);
            _notificationEnabled = (bool)(localSettings.Values["NotificationEnabled"]);
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
