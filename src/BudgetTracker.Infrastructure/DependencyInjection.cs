using BudgetTracker.Application.Common.Interfaces;
using BudgetTracker.Infrastructure.Data;
using BudgetTracker.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BudgetTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                )
            );

            services.AddScoped<IBudgetTrackerDbContext>(provider =>
                provider.GetRequiredService<ApplicationDbContext>());


            services.Configure<Settings.JwtSettings>(
                configuration.GetSection("JwtSettings")
            );
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
