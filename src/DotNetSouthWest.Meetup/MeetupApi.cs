using DotNetSouthWest.Meetup.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static DotNetSouthWest.Meetup.Model.EventsModel;

namespace DotNetSouthWest.Meetup
{
    public class MeetupApi
    {
        public async Task<List<Event>> GetEventsAsync(string urlName)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var json = await http.GetStringAsync($"https://api.meetup.com/{urlName}/events?photo-host=public&page=20");
                    var result = JsonConvert.DeserializeObject<List<Event>>(json);

                    return result;
                }
            }
            catch (Exception e)
            {
                return new List<Event>();
            }   
        }

        public async Task<List<Rsvp>> GetRsvpsAsync(string urlName, string eventId)
        {
            try
            {
                using (var http = new HttpClient())
                {
                    var json = await http.GetStringAsync($"https://api.meetup.com/{urlName}/events/{eventId}/rsvps?photo-host=public");
                    var result = JsonConvert.DeserializeObject<List<Rsvp>>(json);

                    return result;
                }
            }
            catch (Exception e)
            {
                return new List<Rsvp>();
            }
        }

    }
}
