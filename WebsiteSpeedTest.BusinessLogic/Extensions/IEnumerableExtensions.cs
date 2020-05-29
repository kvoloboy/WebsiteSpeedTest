using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RequestSpeedTest.BusinessLogic.Extensions
{
    public static class IEnumerableExtensions
    {
        public static Task ParallelForEachAsync<T>(
            this IEnumerable<T> source,
            Func<T, Task> func,
            int degreeOfParallelism = 4)
        {
            async Task AwaitPartition(IEnumerator<T> partition)
            {
                using (partition)
                {
                    while (partition.MoveNext())
                    {
                        await func(partition.Current);
                    }
                }
            }

            return Task.WhenAll(
                Partitioner.Create(source)
                    .GetPartitions(degreeOfParallelism)
                    .AsParallel()
                    .Select(AwaitPartition));
        }
    }
}
