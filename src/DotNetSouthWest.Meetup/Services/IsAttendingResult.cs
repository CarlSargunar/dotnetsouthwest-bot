using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetSouthWest.Meetup.Model;

namespace DotNetSouthWest.Meetup.Services
{
    public class IsAttendingResult
    {
        private List<Rsvp> _rsvps; 
        public bool HasNextEvent { get; set; }

        public List<Rsvp> MatchingRsvps
        {
            get { return _rsvps ?? (_rsvps = new List<Rsvp>()); }
            set { _rsvps = value; }
        }

        public bool IsTooBroad => MatchingRsvps.Count > 3;
    }
}
