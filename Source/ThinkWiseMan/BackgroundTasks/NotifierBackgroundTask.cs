using DataServices;
using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class NotifierBackgroundTask : IBackgroundTask
    {
        
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastNode = toastXml.SelectSingleNode("toast");
            IXmlNode text = toastXml.GetElementsByTagName("text").FirstOrDefault();
            IXmlDataService xmlDataService = new XmlDataService();

            var ideas = await xmlDataService.GetThoughtsByDayAsync(DateTime.Now.Day,DateTime.Now.Month);
            text.InnerText = ideas.First().Content;
           
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotifier notifier = ToastNotificationManager.CreateToastNotifier("ThinkWiseMan");
            notifier.Show(toast);
            _deferral.Complete();

        }
    }
}
