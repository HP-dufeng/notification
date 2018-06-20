using Abp;
using Abp.Authorization;
using Abp.Notifications;
using Abp.RealTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EWIP.Notification.Demo
{
    public class DemoAppService : AppServiceBase, IDemoAppService
    {
        private readonly IOnlineClientManager _onlineClientManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly INotificationPublisher _notificationPublisher;
        private readonly IUserNotificationManager _userNotificationManager;

        public DemoAppService(
            IOnlineClientManager onlineClientManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            INotificationPublisher notificationPublisher,
            IUserNotificationManager userNotificationManager)
        {
            _onlineClientManager = onlineClientManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _notificationPublisher = notificationPublisher;
            _userNotificationManager = userNotificationManager;
        }

        public object Get()
        {
            return new string[] { "value1", "value2" };
        }

        public object GetAllOnlineClients()
        {
            return _onlineClientManager.GetAllClients();
        }

        [AbpAuthorize]
        public long GetUser()
        {
            return AbpSession.UserId.Value;
        }

        [AbpAuthorize]
        public async Task Subscribe(string notificationName)
        {
            await _notificationSubscriptionManager.SubscribeAsync(GetCurrentUserIdentifier(), notificationName);
        }

        [AbpAuthorize]
        public Task<List<UserNotification>> GetUserNotifications()
        {
            return _userNotificationManager.GetUserNotificationsAsync(GetCurrentUserIdentifier());
        }

        public async Task PublishMessage(long userId, string message)
        {
            var user = new UserIdentifier(1, userId);

            await _notificationPublisher.PublishAsync(
                "App.SimpleMessage",
                new MessageNotificationData(message),
                severity: NotificationSeverity.Info,
                userIds: new[] { user }
            );

        }


    }
}
