using Identity.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.DependencyInjection
{
   public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options => 
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"), 
                b => b.MigrationsAssembly(typeof(Program).Assembly.FullName)));

            return services;
        }
    }
}