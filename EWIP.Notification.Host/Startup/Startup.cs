using System;
using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.EntityFrameworkCore;
using EWIP.Notification.EntityFrameworkCore;
using Castle.Facilities.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EWIP.Notification.Host.Authentication;
using Microsoft.Extensions.Configuration;
using EWIP.Notification.Configuration;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using System.Linq;
using Abp.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.OAuth2Introspection;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using Abp.AspNetCore.SignalR.Hubs;

namespace EWIP.Notification.Host.Startup
{
    public class Startup
    {
        private const string _defaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;

        public Startup(IHostingEnvironment env)
        {
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName);
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Configure DbContext
            services.AddAbpDbContext<NotificationDbContext>(options =>
            {
                DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                options.Filters.Add(new CorsAuthorizationFilterFactory(_defaultCorsPolicyName));
            });



            // Configure CORS for angular2 UI
            services.AddCors(
                options => options.AddPolicy(
                    _defaultCorsPolicyName,
                    builder => builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                )
            );

            services.AddSignalR();
                //.AddMessagePackProtocol();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "IdentityBearer";
            }).AddIdentityServerAuthentication("IdentityBearer", options =>
            {
                options.Authority = _appConfiguration["IdentityServer:Url"];
                options.RequireHttpsMetadata = false;

                options.TokenRetriever = request =>
                {
                    string access_token = TokenRetrieval.FromAuthorizationHeader()(request);

                    if (!string.IsNullOrEmpty(access_token))
                        return access_token;
                    else
                        return TokenRetrieval.FromQueryString()(request);

                };

            });

            // Swagger - Enable this line and the related lines in Configure method to enable swagger UI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "EWIP.Notification API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                // Assign scope requirements to operations based on AuthorizeAttribute
                options.OperationFilter<SecurityRequirementsOperationFilter>();
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<NotificationHostModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAbp(); //Initializes ABP framework.

            app.UseCors(_defaultCorsPolicyName); // Enable CORS!

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseJwtTokenMiddleware("IdentityBearer");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<AbpCommonHub>("/signalr");
            });


            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "EWIP.Notification API V1");
               
                options.IndexStream = () => Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream("EWIP.Notification.Host.wwwroot.swagger.ui.index.html");
            }); // URL: /swagger

        }
    }
}
