using System.Collections.Generic;
using RedisEventStore.Domain;
using RedisEventStore.Utils;
using StackExchange.Redis;

namespace RedisEventStore.EventStore
{
    public class AtomicInsertion : IInsertionStrategy
    {
        public void InsertEvents(IDatabase db, string aggregateId, List<Event> events, int currentVersion)
        {
            var tran = db.CreateTransaction();

            for (int i = 0; i < events.Count; ++i)
            {
                var serializedEvent = ProtoSerializer.Serialize(events[i]);
                tran.SortedSetAddAsync(aggregateId, serializedEvent, events[i].Version);
            }

            db.StringSet($"{aggregateId}-ActualVersion", currentVersion);

            tran.Execute();
        }
    }
}
