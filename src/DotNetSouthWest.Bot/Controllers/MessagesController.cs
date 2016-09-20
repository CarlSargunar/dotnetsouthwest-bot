using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DotNetSouthWest.Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace DotNetSouthWest.Bot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            switch (activity.GetActivityType())
            {
                case ActivityTypes.Message:
                    await Conversation.SendAsync(activity, () => new RootDialog());
                    break;
                
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
