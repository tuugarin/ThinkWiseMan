using DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace ToastTileCreator
{
    public sealed class ToastManager : IToastManager
    {
        IXmlDataService xmlDataService;
        public ToastManager(IXmlDataService _xmlDataService)
        {
            xmlDataService = _xmlDataService;
        }

        public async Task<ToastNotification> CreateToastNotificationAsync(int day, int month)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastNode = toastXml.SelectSingleNode("toast");
            IXmlNode text = toastXml.GetElementsByTagName("text").FirstOrDefault();
            IXmlDataService xmlDataService = new XmlDataService();
            var ideas = await xmlDataService.GetThoughtsByDayAsync(day, month);
            text.InnerText = ideas.First().Content;
            var toast = new ToastNotification(toastXml);
            return toast;
        }
        public async Task<ScheduledToastNotification> CreateSheduledToastNotificationAsync(int day, int month, DateTimeOffset timeToStart)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);
            var toastNode = toastXml.SelectSingleNode("toast");
            IXmlNode text = toastXml.GetElementsByTagName("text").FirstOrDefault();
            IXmlDataService xmlDataService = new XmlDataService();
            var ideas = await xmlDataService.GetThoughtsByDayAsync(day, month);
            text.InnerText = ideas.First().Content;
            var toast = new ScheduledToastNotification(toastXml, timeToStart);
            return toast;
        }
        public async Task ClearAllSheduledNotificationsAsync()
        {
            await Task.Run(() =>
            {
                var toastNotifier = ToastNotificationManager.CreateToastNotifier();
                var notifications = toastNotifier.GetScheduledToastNotifications();
                foreach (var item in notifications)
                {
                    toastNotifier.RemoveFromSchedule(item);
                }
            });

        }
    }
}
