using Abp.BackgroundJobs;
using Abp.EntityFrameworkCore;
using Abp.Notifications;
using Microsoft.EntityFrameworkCore;
using EWIP.Notification.Auditing;

namespace EWIP.Notification.EntityFrameworkCore
{
    public class NotificationDbContext : AbpDbContext
    {
        /// <summary>
        /// Audit logs.
        /// </summary>
        public virtual DbSet<AuditLog> AuditLogs { get; set; }

        /// <summary>
        /// Tenant notifications.
        /// </summary>
        public virtual DbSet<TenantNotificationInfo> TenantNotifications { get; set; }

        /// <summary>
        /// User notifications.
        /// </summary>
        public virtual DbSet<UserNotificationInfo> UserNotifications { get; set; }

        /// <summary>
        /// Notification subscriptions.
        /// </summary>
        public virtual DbSet<NotificationSubscriptionInfo> NotificationSubscriptions { get; set; }

        /// <summary>
        /// Background jobs.
        /// </summary>
        public virtual DbSet<BackgroundJobInfo> BackgroundJobs { get; set; }

        /// <summary>
        /// Notifications.
        /// </summary>
        public virtual DbSet<NotificationInfo> Notifications { get; set; }

        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) 
            : base(options)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuditLog>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.ExecutionTime });
                b.HasIndex(e => new { e.TenantId, e.ExecutionDuration });
            });

            

            modelBuilder.Entity<NotificationSubscriptionInfo>(b =>
            {
                b.HasIndex(e => new { e.NotificationName, e.EntityTypeName, e.EntityId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.NotificationName, e.EntityTypeName, e.EntityId, e.UserId });
            });

            modelBuilder.Entity<TenantNotificationInfo>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<UserNotificationInfo>(b =>
            {
                b.HasIndex(e => new { e.UserId, e.State, e.CreationTime });
            });


            modelBuilder.Entity<BackgroundJobInfo>(b =>
            {
                b.HasIndex(e => new { e.IsAbandoned, e.NextTryTime });
            });

            

            #region AuditLog.Set_MaxLengths

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ServiceName)
                .HasMaxLength(AuditLog.MaxServiceNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.MethodName)
                .HasMaxLength(AuditLog.MaxMethodNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Parameters)
                .HasMaxLength(AuditLog.MaxParametersLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientIpAddress)
                .HasMaxLength(AuditLog.MaxClientIpAddressLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.ClientName)
                .HasMaxLength(AuditLog.MaxClientNameLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.BrowserInfo)
                .HasMaxLength(AuditLog.MaxBrowserInfoLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.Exception)
                .HasMaxLength(AuditLog.MaxExceptionLength);

            modelBuilder.Entity<AuditLog>()
                .Property(e => e.CustomData)
                .HasMaxLength(AuditLog.MaxCustomDataLength);

            #endregion

        }


    }
}
