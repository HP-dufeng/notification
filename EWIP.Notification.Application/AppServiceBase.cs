using Abp;
using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace EWIP.Notification
{
    public abstract class AppServiceBase : ApplicationService
    {
        protected AppServiceBase()
        {
            
        }

        protected virtual UserIdentifier GetCurrentUserIdentifier()
        {
            if (!AbpSession.UserId.HasValue)
            {
                throw new Exception("There is no current user!");
            }

            var user = new UserIdentifier(AbpSession.TenantId, AbpSession.UserId.Value);

            return user;
        }
    }
}
