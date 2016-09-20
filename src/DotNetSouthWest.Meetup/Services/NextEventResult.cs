using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetSouthWest.Meetup.Model;

namespace DotNetSouthWest.Meetup.Services
{
    public class NextEventResult
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime FromMillisecondsSinceUnixEpoch(long milliseconds)
        {
            return UnixEpoch.AddMilliseconds(milliseconds);
        }

        public EventsModel.Event Event { get; set; }
        public bool HasNextEvent { get; set; }
        public DateTime Date => FromMillisecondsSinceUnixEpoch(Event.time);
    }
}
