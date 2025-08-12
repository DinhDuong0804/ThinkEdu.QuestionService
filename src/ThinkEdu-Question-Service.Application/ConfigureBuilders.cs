using Microsoft.AspNetCore.Builder;
using ThinkEdu_Question_Service.Application.Middleware;

namespace ThinkEdu_Question_Service.Application
{
    public static class ConfigureBuilders
    {
        public static IApplicationBuilder AddApplicationBuilders(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}