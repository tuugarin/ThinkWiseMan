using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace ThinkWiseMan.Helpers
{
    public class BackgroundTaskManager : IBackgroundTaskManager
    {
        public async Task RegisterNotificationTaskAsync()
        {
            bool taskRegistred = false;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "ThinkWiseNotificationTask")
                    taskRegistred = true;
            }

            if (!taskRegistred)
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = "ThinkWiseNotificationTask";
                builder.TaskEntryPoint = "BackgroundTasks.NotifierBackgroundTask";
                builder.SetTrigger(new TimeTrigger(24 * 60, false));
                await BackgroundExecutionManager.RequestAccessAsync("ThinkWiseMan");

                var ret = builder.Register();
            }
        }
        public async Task RegisterNotificationTaskUserPersistAsync()
        {
            bool taskRegistred = false;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == "ThinkWiseNotificationTaskUserPersist")
                    taskRegistred = true;
            }

            if (!taskRegistred)
            {
                var builder = new BackgroundTaskBuilder();
                builder.Name = "ThinkWiseNotificationTaskUserPersist";
                builder.TaskEntryPoint = "BackgroundTasks.NotificationBackgroundTaskUserPersist";
                builder.SetTrigger(new SystemTrigger(SystemTriggerType.UserPresent, true));
                
                await BackgroundExecutionManager.RequestAccessAsync("ThinkWiseMan");
                var ret = builder.Register();
            }
        }
        public async Task UnregisterNotificationTaskAsync()
        {
            await Task.Run(() =>
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == "ThinkWiseNotificationTask")
                        task.Value.Unregister(true);
                }

            });
        }
    }
}
