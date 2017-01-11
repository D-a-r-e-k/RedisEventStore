using System;
using ProtoBuf;

namespace RedisEventStore.Domain
{
    [ProtoContract]
    public class Event
    {
        [ProtoMember(1)]
        public int Version { get; set; }
        [ProtoMember(2)]
        public byte[] Data { get; set; }
        [ProtoMember(3)]
        public DateTime Date { get; set; }
    }
}
