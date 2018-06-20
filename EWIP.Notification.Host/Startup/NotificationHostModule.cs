using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EWIP.Notification.Configuration;
using EWIP.Notification.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.AspNetCore.SignalR;

namespace EWIP.Notification.Host.Startup
{
    [DependsOn(
    typeof(NotificationApplicationModule),
    typeof(NotificationEntityFrameworkCoreModule),
    typeof(AbpAspNetCoreModule),
    typeof(AbpAspNetCoreSignalRModule))]
    public class NotificationHostModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public NotificationHostModule(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(NotificationConsts.ConnectionStringName);


            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(NotificationApplicationModule).GetAssembly()
                );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NotificationHostModule).GetAssembly());
        }
    }
}