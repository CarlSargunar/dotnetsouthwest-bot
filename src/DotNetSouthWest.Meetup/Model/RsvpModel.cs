using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetSouthWest.Meetup.Model
{ 
    public class Rsvp
    {
        public long created { get; set; }
        public long updated { get; set; }
        public string response { get; set; }
        public int guests { get; set; }
        public Member member { get; set; }
    }

    public class Member
    {
        public int id { get; set; }
        public string name { get; set; }
        public string bio { get; set; }
        public Photo photo { get; set; }
        public string role { get; set; }
    }

    public class Photo
    {
        public int id { get; set; }
        public string highres_link { get; set; }
        public string photo_link { get; set; }
        public string thumb_link { get; set; }
        public string type { get; set; }
        public string base_url { get; set; }
    }

}
