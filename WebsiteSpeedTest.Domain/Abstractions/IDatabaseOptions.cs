namespace RequestSpeedTest.Domain.Abstractions
{
    public interface IDatabaseOptions
    {
        string GetDatabaseName();
        string GetCollectionName<TEntity>();
    }
}
