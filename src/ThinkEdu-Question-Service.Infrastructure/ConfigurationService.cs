using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using ThinkEdu_Question_Service.Application.Common;
using ThinkEdu_Question_Service.Application.Common.Interfaces.Services;
using ThinkEdu_Question_Service.Domain.Configurations;
using ThinkEdu_Question_Service.Domain.Extensions;
using ThinkEdu_Question_Service.Infrastructure.Common;
using ThinkEdu_Question_Service.Infrastructure.Persistence;
using ThinkEdu_Question_Service.Infrastructure.Services;

namespace ThinkEdu_Question_Service.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var databaseSettings = services.GetOptions<DatabaseSettings>(nameof(DatabaseSettings));
            if (databaseSettings == null || string.IsNullOrEmpty(databaseSettings.ConnectionString))
                throw new ArgumentNullException("Connection string is not configured.");

            services.AddDbContext<ThinkEduContext>(option =>
            {
                option.UseNpgsql(databaseSettings.ConnectionString, builder =>
                    builder.MigrationsAssembly(typeof(ThinkEduContext).Assembly.FullName));
            });

            
            #region   Đăng ký Cloudinary config

            services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
            services.AddScoped<CloudinaryService>();
            #endregion

            services
                .AddSingleton<IDataSourceService, DataSourceService>();

            services
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IFunctionHelper, FunctionHelper>();
          
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders();
                    });
            });

            services.RegisterAssemblyServices();

            return services;
        }

        public static IHost MigrateDatabase(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ThinkEduContext>();
            dbContext.Database.Migrate();
            return host;
        }

        private static IServiceCollection RegisterAssemblyServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var serviceTypes = assembly.GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract);

            foreach (var serviceType in serviceTypes)
            {
                var interfaces = serviceType.GetInterfaces();
                var mainInterface = interfaces.FirstOrDefault(i =>
                    (i.Name.StartsWith("I") && i.Name.EndsWith("Service"))
                    || (i.Name.StartsWith("I") && i.Name.EndsWith("Repository"))
                );
                if (mainInterface != null)
                    services.AddScoped(mainInterface, serviceType);
            }

            return services;
        }
    }
}