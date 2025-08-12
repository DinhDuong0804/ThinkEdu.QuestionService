using Microsoft.Extensions.Hosting;
using Serilog.Events;
using Serilog;

namespace ThinkEdu_Question_Service.Domain.Extensions
{
    public class SeriloggerExtension
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
       (context, configuration) =>
       {
           if (context.HostingEnvironment.IsProduction())
               configuration.MinimumLevel.Information();
           else
               configuration.MinimumLevel.Debug();
           configuration.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
           configuration.MinimumLevel.Override("Quartz", LogEventLevel.Information);

           var applicationName = context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-");
           var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";

           configuration
               .WriteTo.Debug()
               .WriteTo.Console(outputTemplate:
                   "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
               .WriteTo.File("Logs/.txt", rollingInterval: RollingInterval.Day)
               .Enrich.FromLogContext()
               .Enrich.WithMachineName()
               .Enrich.WithProperty("Environment", environmentName)
               .Enrich.WithProperty("Application", applicationName)
               .ReadFrom.Configuration(context.Configuration);
       };
    }
}