using DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class NotificationBackgroundTaskUserPersist : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if ((bool)localSettings.Values["NotificationEnabled"])
            {
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
                var toastNode = toastXml.SelectSingleNode("toast");
                IXmlNode text = toastXml.GetElementsByTagName("text").FirstOrDefault();
                IXmlDataService xmlDataService = new XmlDataService();
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                var ideas = await xmlDataService.GetThoughtsByDayAsync(DateTime.Now.Day, DateTime.Now.Month);
                text.InnerText = ideas.First().Content;
                var toast = new ToastNotification(toastXml);
                toastNotifier.Show(toast);
            }
            _deferral.Complete();
        }
    }
}
