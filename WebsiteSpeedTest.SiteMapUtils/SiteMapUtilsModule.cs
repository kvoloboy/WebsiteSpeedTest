using Autofac;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;
using RequestSpeedTest.SiteMapUtils.Factories;
using RequestSpeedTest.SiteMapUtils.Factories.Interfaces;
using RequestSpeedTest.SiteMapUtils.Models;
using RequestSpeedTest.SiteMapUtils.Services;
using TurnerSoftware.SitemapTools.Parser;

namespace RequestSpeedTest.SiteMapUtils
{
    public class SiteMapUtilsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SiteMapService>()
                .As<ISiteMapService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<XmlSitemapParser>()
                .Named<ISitemapParser>(ContentTypes.ApplicationXml)
                .InstancePerLifetimeScope();

            builder.RegisterType<XmlSitemapParser>()
                .Named<ISitemapParser>(ContentTypes.TextXml)
                .InstancePerLifetimeScope();

            builder.RegisterType<TextSitemapParser>()
                .Named<ISitemapParser>(ContentTypes.TextPlain)
                .InstancePerLifetimeScope();

            builder.RegisterType<SitemapParserFactory>()
                .As<ISitemapParserFactory>()
                .InstancePerLifetimeScope();
        }
    }
}
