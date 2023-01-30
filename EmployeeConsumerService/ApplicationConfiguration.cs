﻿using EmployeeConsumerService.Hubs;

namespace EmployeeConsumerService
{
    public static class ApplicationConfiguration
    {
        #region Public Methods

        public static void AddApplicationConfiguration(this WebApplication app, IConfiguration configuration)
        {
            string employeeConsumerSignalRHubUrl = configuration.GetValue<string>("employeeConsumerSignalRHubUrl");

            //app.UseCors("corsapp");

            app.UseRouting();

            string signalRUrl = string.Concat("/", employeeConsumerSignalRHubUrl);

            app.UseEndpoints(x =>
            {
                x.MapHub<EmployeeConsumerSignalHub>(signalRUrl);
            });

            app.UseXContentTypeOptions();

            app.UseReferrerPolicy(options => options.StrictOriginWhenCrossOrigin());

            app.UseCsp(options => options.FrameAncestors(s => s.Self()));

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Permissions-Policy", "geolocation=(), midi=(),sync-xhr=(),accelerometer=(), gyroscope=(), magnetometer=(), camera=(), fullscreen=(self)");
                await next();
            });

            app.UseXfo(options => options.SameOrigin());

            app.UseXXssProtection(options => options.EnabledWithBlockMode());
        }

        #endregion Public Methods
    }
}
