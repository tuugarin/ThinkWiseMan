using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace ToastTileCreator
{
    public interface IToastManager
    {
        Task<ScheduledToastNotification> CreateSheduledToastNotificationAsync(int day, int month, DateTimeOffset timeToStart);
        Task<ToastNotification> CreateToastNotificationAsync(int day, int month);
        Task ClearAllSheduledNotificationsAsync();
    }
}