using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWIP.Notification
{
    [DependsOn(
    typeof(NotificationCoreModule),
    typeof(AbpAutoMapperModule))]
    public class NotificationApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NotificationApplicationModule).GetAssembly());
        }
    }
}
