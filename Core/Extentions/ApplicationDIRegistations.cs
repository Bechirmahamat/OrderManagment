using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extentions
{
    public static class ApplicationDIRegistations
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
