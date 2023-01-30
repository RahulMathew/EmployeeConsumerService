using Microsoft.Extensions.Configuration;

namespace EmployeeConsumerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;

            builder.Services.RegisterServices(configuration);

            var app = builder.Build();

            app.AddApplicationConfiguration(configuration);

            app.MapGet("/", () => "Employee Consumer Service Starting");

            app.Run();
        }
    }
}
