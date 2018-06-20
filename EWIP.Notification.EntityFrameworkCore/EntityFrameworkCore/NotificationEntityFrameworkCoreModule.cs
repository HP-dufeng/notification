using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace EWIP.Notification.EntityFrameworkCore
{
    [DependsOn(
        typeof(NotificationCoreModule), 
        typeof(AbpEntityFrameworkCoreModule))]
    public class NotificationEntityFrameworkCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NotificationEntityFrameworkCoreModule).GetAssembly());
        }
    }
}