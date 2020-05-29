using Autofac;
using RequestSpeedTest.SiteMapUtils.Factories.Interfaces;
using TurnerSoftware.SitemapTools.Parser;

namespace RequestSpeedTest.SiteMapUtils.Factories
{
    public class SitemapParserFactory : ISitemapParserFactory
    {
        private readonly ILifetimeScope _lifetimeScope;

        public SitemapParserFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public ISitemapParser Create(string sitemapType)
        {
            var parser = _lifetimeScope.ResolveNamed<ISitemapParser>(sitemapType);

            return parser;
        }
    }
}
