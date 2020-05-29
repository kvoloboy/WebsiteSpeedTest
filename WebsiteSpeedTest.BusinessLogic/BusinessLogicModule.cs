using Autofac;
using RequestSpeedTest.BusinessLogic.Services;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;

namespace RequestSpeedTest.BusinessLogic
{
    public class BusinessLogicModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<WebsiteSpeedStatisticService>()
                .As<IWebsiteSpeedStatisticService>()
                .InstancePerLifetimeScope();
        }
    }
}
