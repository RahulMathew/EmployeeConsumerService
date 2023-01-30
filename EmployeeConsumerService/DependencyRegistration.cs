using BusinessLayer.Core;
using BusinessLayer.Interfaces;
using Common.AdoUtils.Core;
using Common.AdoUtils.Interfaces;
using Common.ServiceBus.Core;
using Common.ServiceBus.Interfaces;
using DBContextLayer.Core;
using DBContextLayer.Interfaces;
using EmployeeConsumerService.HostedServices.Core;
using EmployeeConsumerService.HostedServices.Interfaces;

namespace EmployeeConsumerService
{
    public static class DependencyRegistration
    {
        #region Public Methods

        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSignalR();

            //services.AddCors(p => p.AddPolicy("corsapp", builder =>
            //{
            //    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            //}));

            string confluenceKafkaServiceBusUrl = configuration.GetValue<string>("ConfluenceKafkaServiceBusUrl");
            string employeeServiceTopicName = configuration.GetValue<string>("EmployeeServiceTopicName");
            string employeeServiceTopicGroupId = configuration.GetValue<string>("EmployeeServiceTopicGroupId");

            var topicConsumer = new ConfluentKafkaTopicConsumer(confluenceKafkaServiceBusUrl, employeeServiceTopicName, employeeServiceTopicGroupId);
            services.AddSingleton<ITopicConsumer>(topicConsumer);

            SqlServerDal sqlServerDal = new SqlServerDal();
            sqlServerDal.ConnectionString = configuration.GetValue<string>("SqlConnectionString");
            services.AddSingleton<IAdoDal>(sqlServerDal);
            services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton<IEmployeeDBContext, EmployeeDBContext>();
            services.AddSingleton<IEmployeeConsumerSignalRBroadcaster, EmployeeConsumerSignalRBroadcaster>();

            services.AddHostedService<EmployeeConsumerBackgroundService>();

            return services;
        }

        #endregion Public Methods
    }
}
