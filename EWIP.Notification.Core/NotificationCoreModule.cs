using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWIP.Notification
{
    public class NotificationCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Auditing.IsEnabledForAnonymousUsers = true;

            Configuration.MultiTenancy.IsEnabled = false;

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NotificationCoreModule).GetAssembly());
        }
    }
}
