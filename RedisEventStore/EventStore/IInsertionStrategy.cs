using System;
using System.Collections.Generic;
using RedisEventStore.Domain;
using StackExchange.Redis;

namespace RedisEventStore.EventStore
{
    public interface IInsertionStrategy
    {
        void InsertEvents(IDatabase db, string aggregateId, List<Event> events, int currentVersion);
    }
}
