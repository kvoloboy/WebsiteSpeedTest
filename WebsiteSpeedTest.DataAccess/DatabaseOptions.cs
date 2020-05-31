using Microsoft.Extensions.Configuration;
using RequestSpeedTest.Domain.Abstractions;

namespace WebsiteSpeedTest.DataAccess
{
    public class DatabaseOptions : IDatabaseOptions
    {
        private const string ConfigSegmentName = "DatabaseOptions";

        private readonly IConfiguration _configuration;

        public DatabaseOptions(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetDatabaseName()
        {
            const string databaseNameSegment = "DatabaseName";
            var databaseName = _configuration[$"{ConfigSegmentName}:{databaseNameSegment}"];

            return databaseName;
        }

        public string GetCollectionName<TEntity>()
        {
            const string collectionsSegment = "Collections";
            var targetCollectionKey = typeof(TEntity).Name;
            var targetCollectionValue =
                _configuration[$"{ConfigSegmentName}:{collectionsSegment}:{targetCollectionKey}"];

            return targetCollectionValue;
        }
    }
}
