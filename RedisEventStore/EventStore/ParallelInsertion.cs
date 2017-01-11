using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedisEventStore.Domain;
using RedisEventStore.Utils;
using StackExchange.Redis;

namespace RedisEventStore.EventStore
{
    public class ParallelInsertion : IInsertionStrategy
    {
        public void InsertEvents(IDatabase db, string aggregateId, List<Event> events, int currentVersion)
        {
            Parallel.For(0, events.Count, i =>
            {
                var serializedEvent = ProtoSerializer.Serialize(events[i]);
                db.SortedSetAddAsync(aggregateId.ToString(), serializedEvent, events[i].Version);
            });

            db.StringSet($"{aggregateId}-ActualVersion", currentVersion);
        }
    }
}
