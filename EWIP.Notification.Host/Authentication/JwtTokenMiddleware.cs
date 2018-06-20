using Abp.Runtime.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EWIP.Notification.Host.Authentication
{
    public static class JwtTokenMiddleware
    {
        public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder app)
        {
            return UseJwtTokenMiddleware(app, JwtBearerDefaults.AuthenticationScheme);
        }

        public static IApplicationBuilder UseJwtTokenMiddleware(this IApplicationBuilder app, string authenticationScheme)
        {
            return app.Use(async (ctx, next) =>
            {
                if (ctx.User.Identity?.IsAuthenticated != true)
                {
                    var result = await ctx.AuthenticateAsync(authenticationScheme);
                    if (result.Succeeded && result.Principal != null)
                    {
                        ctx.User = result.Principal;
                        ctx.User.AddSubIdentity();
                    }
                }
                else
                {
                    ctx.User.AddSubIdentity();
                }

                await next();
            });
        }

        public static void AddSubIdentity(this ClaimsPrincipal claim)
        {
            
            var subClaim = claim.Claims.FirstOrDefault(p => p.Type == "sub");
            if (subClaim != null && !claim.HasClaim(p => p.Type == AbpClaimTypes.UserId))
            {
                var userIdClaim = claim.Claims.FirstOrDefault(p => p.Type == "UserId");
                claim.AddIdentity(new ClaimsIdentity(new List<Claim>
                {
                    new Claim(AbpClaimTypes.UserId,
                        userIdClaim.Value.ToString())
                }));
            }
        }

    }
}
