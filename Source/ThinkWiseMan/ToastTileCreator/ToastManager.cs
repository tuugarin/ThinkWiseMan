using DataServices;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;
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
        ISqlDataService dataService;
        public ToastManager(ISqlDataService _dataService)
        {
            dataService = _dataService;
        }

        public async Task<ToastNotification> CreateToastNotificationAsync(int day, int month)
        {
            var ideas = await dataService.GetThoughtsByDayAsync(day, month);
            XmlDocument toastXml = GenerateToastContent(ideas.First().Content, "Мысли мудрых", day, month);
            var toast = new ToastNotification(toastXml);
            return toast;
        }
        public async Task<ScheduledToastNotification> CreateSheduledToastNotificationAsync(int day, int month, DateTimeOffset timeToStart)
        {
            var ideas = await dataService.GetThoughtsByDayAsync(day, month);
            XmlDocument toastXml = GenerateToastContent(ideas.First().Content, "Мысли мудрых", day, month);
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
        XmlDocument GenerateToastContent(string text, string title, int day, int month)
        {
            ToastContent content = new ToastContent()
            {

                Launch = new QueryString() {
                    { "day", day.ToString() },
                    { "month", month.ToString() }
                }.ToString(),
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                                {
                                        new AdaptiveText()
                                        {
                                        Text = title,
                                        HintStyle = AdaptiveTextStyle.Title
                                        },

                                        new AdaptiveText()
                                        {
                                        Text = text,
                                        HintStyle = AdaptiveTextStyle.Body
                                        }
                                        }


                    }
                }
            };
            return content.GetXml();
        }
    }
}
