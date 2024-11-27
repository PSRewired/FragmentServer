using System;
using System.Collections.Generic;
using System.Linq;

namespace Fragment.NetSlum.Networking.OnlineEvents
{
    public class EventData
    {
        // List of events with updated date ranges
        public static List<Event> Events = new List<Event>
        {
            new Event { Name = "Christmas", StartDate = new DateTime(2005, 12, 24), EndDate = new DateTime(2005, 12, 25) },
            new Event { Name = "New Years Eve", StartDate = new DateTime(2005, 12, 30), EndDate = new DateTime(2005, 12, 31) },
            new Event { Name = "New Years Day", StartDate = new DateTime(2006, 01, 01), EndDate = new DateTime(2006, 01, 02) },
            new Event { Name = "Setsubun", StartDate = new DateTime(2006, 02, 03), EndDate = new DateTime(2006, 02, 03) },
            new Event { Name = "Valentine's Day", StartDate = new DateTime(2006, 02, 13), EndDate = new DateTime(2006, 02, 13) },
            new Event { Name = "Cherry Blossom", StartDate = new DateTime(2006, 03, 15), EndDate = new DateTime(2006, 03, 15) },
            new Event { Name = "Star Festival", StartDate = new DateTime(2006, 07, 07), EndDate = new DateTime(2006, 07, 07) }, // Tanabata
            new Event { Name = "Obon", StartDate = new DateTime(2006, 08, 13), EndDate = new DateTime(2006, 08, 15) },
            new Event { Name = "PC Day", StartDate = new DateTime(2006, 09, 29), EndDate = new DateTime(2006, 09, 29) },
            new Event { Name = "Sports Day", StartDate = new DateTime(2006, 10, 09), EndDate = new DateTime(2006, 10, 09) }
        };

        public class Event
        {
            public string? Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; } // Nullable in case the event is a single day
        }
    }
}
