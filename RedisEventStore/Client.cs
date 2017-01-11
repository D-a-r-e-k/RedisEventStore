using System;
using System.Collections.Generic;
using RedisEventStore.Domain;
using RedisEventStore.Utils;

namespace RedisEventStore
{
    public class Client
    {
        public static void Main(string[] args)
        {
            using (var eventStore = new EventStore.EventStore())
            {
                //var eventsToBeInserted = new List<Event>
                //{
                //    new Event()
                //    {
                //        Version = 0,
                //        Data = ProtoSerializer.Serialize("event1"),
                //        Date = DateTime.Now
                //    },
                //    new Event()
                //    {
                //        Version = 0,
                //        Data = ProtoSerializer.Serialize("event2"),
                //        Date = DateTime.Now
                //    }
                //};

                var eventsToBeInserted = new List<Event>
                {
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event1"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event2"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event3"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event4"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event5"),
                        Date = DateTime.Now
                    },new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event6"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event7"),
                        Date = DateTime.Now
                    },new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event8"),
                        Date = DateTime.Now
                    },
                    new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event9"),
                        Date = DateTime.Now
                    },new Event()
                    {
                        Version = 0,
                        Data = ProtoSerializer.Serialize("event10"),
                        Date = DateTime.Now
                    }
                };

                //new Guid("0eaf40de-6481-4895-a654-e6bea1c6a594")
                for (int i = 0; i < 10; ++i)
                {
                    int expectedVersion = i*10;
                    eventStore.SaveEvents(new Guid("0eaf40de-6481-4895-a654-e6bea1c6a594"), expectedVersion,
                        eventsToBeInserted);
                }
            }

            Console.WriteLine("Work done.");            
        }
    }
}
