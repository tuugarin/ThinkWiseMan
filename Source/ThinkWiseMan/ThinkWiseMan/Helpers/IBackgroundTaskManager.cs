using System.Threading.Tasks;

namespace ThinkWiseMan.Helpers
{
    public interface IBackgroundTaskManager
    {
        Task RegisterNotificationTask();
    }
}