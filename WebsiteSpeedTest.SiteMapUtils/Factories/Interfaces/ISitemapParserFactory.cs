using TurnerSoftware.SitemapTools.Parser;

namespace RequestSpeedTest.SiteMapUtils.Factories.Interfaces
{
    public interface ISitemapParserFactory
    {
        ISitemapParser Create(string sitemapType);
    }
}
