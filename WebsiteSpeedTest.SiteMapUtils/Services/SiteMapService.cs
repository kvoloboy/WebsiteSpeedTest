using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using RequestSpeedTest.BusinessLogic.Services.Interfaces;
using RequestSpeedTest.SiteMapUtils.Factories.Interfaces;
using RequestSpeedTest.SiteMapUtils.Models;
using TurnerSoftware.RobotsExclusionTools;
using TurnerSoftware.SitemapTools;

namespace RequestSpeedTest.SiteMapUtils.Services
{
    public class SiteMapService : ISiteMapService
    {
        private const string DefaultSiteMapPath = "sitemap.xml";

        private readonly ISitemapParserFactory _sitemapParserFactory;
        private readonly HttpClient _httpClient;

        public SiteMapService(ISitemapParserFactory sitemapParserFactory)
        {
            _sitemapParserFactory = sitemapParserFactory;

            var clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _httpClient = new HttpClient(clientHandler);
        }

        public async Task<IEnumerable<Uri>> GetSiteUrisAsync(Uri siteUri)
        {
            var sitemapFiles = new Dictionary<Uri, SitemapFile>();
            var sitemapUris = new Stack<Uri>(DiscoverSiteMapsAsync(siteUri.Scheme, siteUri.Host));

            while (sitemapUris.Any())
            {
                var sitemapUri = sitemapUris.Pop();
                var sitemapFile = await GetSitemapAsync(sitemapUri);

                if (sitemapFile == null)
                {
                    continue;
                }

                sitemapFiles.Add(sitemapUri, sitemapFile);

                foreach (var indexFile in sitemapFile.Sitemaps)
                {
                    if (!sitemapFiles.ContainsKey(indexFile.Location))
                    {
                        sitemapUris.Push(indexFile.Location);
                    }
                }
            }

            var uris = sitemapFiles.Values.Where(file => file.Urls != null)
                .SelectMany(file => file.Urls.Select(u => u.Location));

            return uris;
        }

        private IEnumerable<Uri> DiscoverSiteMapsAsync(string schemaName, string domainName)
        {
            var uriBuilder = new UriBuilder(schemaName, domainName)
            {
                Path = DefaultSiteMapPath
            };

            var defaultSitemapUri = uriBuilder.Uri;

            var sitemapUris = new List<Uri>
            {
                defaultSitemapUri
            };

            return sitemapUris.Distinct();
        }

        private async Task<SitemapFile> GetSitemapAsync(Uri sitemapUrl)
        {
            var response = await GetResponseAsync(sitemapUrl);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var contentType = response.Content.Headers.ContentType.MediaType;

            ThrowIfNotValidContentType(contentType);

            var requiresManualDecompression = false;

            if (contentType.Equals(ContentTypes.ApplicationGZip, StringComparison.InvariantCultureIgnoreCase))
            {
                requiresManualDecompression = true;
                var baseFileName = Path.GetFileNameWithoutExtension(sitemapUrl.AbsolutePath);
                contentType = MimeTypes.GetMimeType(baseFileName);
            }

            var sitemapFile = await ParseResponseAsSitemapFileAsync(response, requiresManualDecompression, contentType);
            sitemapFile.Location = sitemapUrl;

            return sitemapFile;
        }

        private async Task<HttpResponseMessage> GetResponseAsync(Uri sitemapUrl)
        {
            HttpResponseMessage response;
            try
            {
                response = await _httpClient.GetAsync(sitemapUrl);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    return null;
                }

                throw;
            }

            return response;
        }

        private void ThrowIfNotValidContentType(string contentType)
        {
            if (!ContentTypes.IsAvailable(contentType))
            {
                throw new InvalidOperationException($"Unknown sitemap content type {contentType}");
            }
        }

        private async Task<SitemapFile> ParseResponseAsSitemapFileAsync(
            HttpResponseMessage response,
            bool requiresManualDecompression,
            string contentType)
        {
            var parser = _sitemapParserFactory.Create(contentType);

            await using var stream = await response.Content.ReadAsStreamAsync();
            var contentStream = stream;

            if (requiresManualDecompression)
            {
                contentStream = new GZipStream(contentStream, CompressionMode.Decompress);
            }

            using var streamReader = new StreamReader(contentStream);
            var sitemap = parser.ParseSitemap(streamReader);

            return sitemap;
        }
    }
}
