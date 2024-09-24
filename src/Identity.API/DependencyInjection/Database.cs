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
                    sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null)));
                //b => b.MigrationsAssembly(typeof(Program).Assembly.FullName));

            return services;
        }
    }
}