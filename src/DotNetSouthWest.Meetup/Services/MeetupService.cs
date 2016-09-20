using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DotNetSouthWest.Meetup.Model;

namespace DotNetSouthWest.Meetup.Services
{
    [Serializable]
    public class MeetupService
    {
        public async Task<NextEventResult> GetNextEventAsync()
        {
            var result = new NextEventResult();
            var events = await new MeetupApi().GetEventsAsync("dotnetsouthwest");
            if (!events.Any())
            {
                result.HasNextEvent = false;
                return result;
            }

            result.Event = events.FirstOrDefault();
            result.HasNextEvent = true;

            return result;
        }

        public async Task<IsAttendingResult> GetIsPersonAttendingAsync(string name)
        {
            var result = new IsAttendingResult();
            var nextEvent = await GetNextEventAsync();
            if (!nextEvent.HasNextEvent)
            {
                result.HasNextEvent = false;
            }

            result.HasNextEvent = true;
            var rsvps = await new MeetupApi().GetRsvpsAsync("dotnetsouthwest", nextEvent.Event.id);
            var potentials = rsvps.Where(r => r.member.name.ToLower().StartsWith(name.ToLower()));
            result.MatchingRsvps = potentials.ToList();
            return result;
        }

        public async Task<Rsvp> PickRaffleWinnerAsync()
        {
            var events = await new MeetupApi().GetEventsAsync("dotnetsouthwest");
            var nextEvent = events.FirstOrDefault();
            var rsvps = await new MeetupApi().GetRsvpsAsync("dotnetsouthwest", nextEvent.id);

            var random = new Random();
            var winner = rsvps.ElementAt(random.Next(rsvps.Count));

            return winner;
        }
    }
}
