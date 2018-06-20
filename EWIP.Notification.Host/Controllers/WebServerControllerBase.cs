using Abp.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWIP.Notification.Host.Controllers
{
    public abstract class WebServerControllerBase : AbpController
    {
        protected WebServerControllerBase()
        {

        }
    }
}
