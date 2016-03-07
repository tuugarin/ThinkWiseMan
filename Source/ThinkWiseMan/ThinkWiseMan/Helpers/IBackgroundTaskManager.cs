using System.Threading.Tasks;

namespace ThinkWiseMan.Helpers
{
    public interface IBackgroundTaskManager
    {
        Task RegisterNotificationTaskAsync();
        Task UnregisterNotificationTaskAsync();
    }
}