using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using RequestSpeedTest.API.Models;

namespace RequestSpeedTest.API.Configurations
{
    public class CorsConfiguration
    {
        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            var corsOptions = configuration.GetSection(nameof(CorsOptions)).Get<CorsOptions>();

            app.UseCors(builder =>
            {
                builder.WithMethods(corsOptions.AllowedMethods);
                builder.WithHeaders(corsOptions.AllowedHeaders);
                builder.WithOrigins(corsOptions.AllowedOrigins);
            });
        }
    }
}
