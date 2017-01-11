using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using ProtoBuf;
using RedisEventStore.Domain;
using RedisEventStore.Utils;
using StackExchange.Redis;

namespace RedisEventStore.EventStore
{
    public class EventStore : IDisposable
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        private readonly IInsertionStrategy _insertionStrategy;

        public EventStore()
        {
            _connectionMultiplexer = ConnectionMultiplexer.Connect("localhost,abortConnect=false");
            _insertionStrategy = new AtomicInsertion();
        }

        public List<Event> RetrieveForAggregate(string aggregateId)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var members = db.SortedSetRangeByScore(aggregateId);

            return members.Select(x =>
            {
                var stream = new MemoryStream(x);
                return Serializer.Deserialize<Event>(stream);
            })
            .ToList();
        }

        private int GetEventSourceVersionDirectly(string aggregateId)
        {
            var db = _connectionMultiplexer.GetDatabase();

            long eventsCount = db.SortedSetLength(aggregateId);

            if (eventsCount == 0)
                return 0;

            var lastEvent = db.SortedSetRangeByRank(aggregateId, eventsCount - 1).FirstOrDefault();
            var deserializedLastEvent = ProtoSerializer.Deserialize<Event>(lastEvent);

            return deserializedLastEvent.Version;
        }

        private int GetEventSourceVersionThroughKey(string aggregateId)
        {
            var db = _connectionMultiplexer.GetDatabase();

            var aggregateVersion = db.StringGet($"{aggregateId}-ActualVersion");

            if (aggregateVersion == RedisValue.Null)
                return 0;
            return (int)aggregateVersion;
        }        

        public void SaveEvents(Guid aggregateId, int expectedVersion, List<Event> events)
        {
            var db = _connectionMultiplexer.GetDatabase();

            int currentVersion = GetEventSourceVersionThroughKey(aggregateId.ToString());

            if (expectedVersion != currentVersion)
                throw new DBConcurrencyException("Concurrency problem.");

            foreach (var e in events)
                e.Version = ++currentVersion;

            _insertionStrategy.InsertEvents(db, aggregateId.ToString(), events, currentVersion);            
        }

        public void Dispose()
        {
            _connectionMultiplexer.Dispose();
        }
    }
}
