using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;

namespace WebsiteSpeedTest.DataAccess.Repositories
{
    public class RequestBenchmarkEntryRepository : IRepository<RequestBenchmarkEntry>
    {
        private readonly IMongoCollection<RequestBenchmarkEntry> _requestCollection;

        public RequestBenchmarkEntryRepository(IMongoClient client, IDatabaseOptions options)
        {
            var databaseName = options.GetDatabaseName();
            var collectionName = options.GetCollectionName<RequestBenchmarkEntry>();

            var database = client.GetDatabase(databaseName);
            _requestCollection = database.GetCollection<RequestBenchmarkEntry>(collectionName);
        }

        public async Task<RequestBenchmarkEntry> FindSingleAsync(
            Expression<Func<RequestBenchmarkEntry, bool>> predicate)
        {
            var request = (await _requestCollection.FindAsync(predicate)).FirstOrDefault();

            return request;
        }

        public async Task<List<RequestBenchmarkEntry>> FindAllAsync(
            Expression<Func<RequestBenchmarkEntry, bool>> predicate = null)
        {
            return predicate != null
                ? (await _requestCollection.FindAsync(predicate)).ToList()
                : (await _requestCollection.FindAsync(entry => true)).ToList();
        }

        public async Task AddAsync(RequestBenchmarkEntry entity)
        {
            var maxId = await _requestCollection.CountDocumentsAsync(Builders<RequestBenchmarkEntry>.Filter.Empty);
            entity.Id = ++maxId;

            await _requestCollection.InsertOneAsync(entity);
        }
    }
}
