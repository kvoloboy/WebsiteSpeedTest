using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RequestSpeedTest.API.Configurations;
using RequestSpeedTest.API.Mappings;
using RequestSpeedTest.BusinessLogic;
using RequestSpeedTest.BusinessLogic.Mappings;
using RequestSpeedTest.SiteMapUtils;
using WebsiteSpeedTest.DataAccess;

namespace RequestSpeedTest.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(ExceptionFilter));
            });

            services.AddAutoMapper(
                typeof(DtoToViewModelProfile),
                typeof(EntityToDtoProfile));
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new BusinessLogicModule());
            builder.RegisterModule(new DataAccessModule());
            builder.RegisterModule(new SiteMapUtilsModule());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            CorsConfiguration.Configure(app, Configuration);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
