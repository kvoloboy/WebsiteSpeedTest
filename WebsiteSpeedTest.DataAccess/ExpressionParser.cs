using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace WebsiteSpeedTest.DataAccess
{
    public static class ExpressionParser
    {
        public static BsonDocument GetBsonDocumentFilter<TEntity>(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return new BsonDocument();
            }

            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var serializer = serializerRegistry.GetSerializer<TEntity>();
            var filter = Builders<TEntity>.Filter.Where(predicate);
            var result = filter.Render(serializer, serializerRegistry).AsBsonDocument;

            return result;
        }
    }
}
