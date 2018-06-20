using Abp.Application.Services;
using Abp.Notifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EWIP.Notification.Demo
{
    public interface IDemoAppService : IApplicationService
    {
        object Get();

        long GetUser();

        object GetAllOnlineClients();

        Task Subscribe(string notificationName);

        Task PublishMessage(long userId, string message);

        Task<List<UserNotification>> GetUserNotifications();
    }
}
