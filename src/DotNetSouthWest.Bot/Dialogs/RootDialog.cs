using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using DotNetSouthWest.Meetup;
using DotNetSouthWest.Bot.CardFactories;
using DotNetSouthWest.Meetup.Services;

namespace DotNetSouthWest.Bot.Dialogs
{
    [Serializable]
    [LuisModel("a43b17f1-02d6-41f3-8424-c3ab1fe4b785", "8c1452437cc14ea4ad2343e98d98a553")]
    public class RootDialog : LuisDialog<object>
    {
        private MeetupService _meetupService;

        public RootDialog()
        {
            _meetupService = new MeetupService();
        }

        [LuisIntent("")]
        public async Task HandleNone(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I didn't understand what you meant.");
            await context.PostAsync(result.Query);
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task HandleGreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hey there!");
            await context.PostAsync("I can tell you all about upcoming .NET South West events.");
            await context.PostAsync(@"Ask me things like ""When is the next event?"" or perhaps ""Is Benjamin attending?""");
            context.Wait(MessageReceived);
        }

        [LuisIntent("NextEventQuery")]
        public async Task HandleNextEventIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Hmmm... Let me just take a look for you..");
            var nextMeetup = await _meetupService.GetNextEventAsync();
            if (!nextMeetup.HasNextEvent)
            {
                await context.PostAsync("Looks like there are no upcoming events at the moment.");
                context.Wait(MessageReceived);
                return;
            }
            
            await context.PostAsync($@"The next event is titled ""{nextMeetup.Event.name}"" and is going to be held at ""{nextMeetup.Event.venue.name}"" on {nextMeetup.Date.ToShortDateString()}");
            context.Wait(MessageReceived);
        }

        [LuisIntent("AttendanceQuery")]
        public async Task HandleAttendanceQueryIntent(IDialogContext context, LuisResult result)
        {
            EntityRecommendation name;
            if (!result.TryFindEntity("AttendeeName", out name))
            {
                name = new EntityRecommendation(type: "AttendeeName") { Entity = "" };
            }
            
            await context.PostAsync("Let me take a look for you...");
            var meetupResult = await _meetupService.GetIsPersonAttendingAsync(name.Entity);
            if (!meetupResult.HasNextEvent)
            {
                await context.PostAsync("There are no upcoming events.");
                context.Wait(MessageReceived);
                return;
            }
            
            // Query is to broad (too many people by that name)
            if (meetupResult.IsTooBroad)
            {
                await context.PostAsync("You are going to have to narrow this down for me a little :)");
                context.Wait(MessageReceived);
                return;
            }

            // Only 1 match.
            if (meetupResult.MatchingRsvps.Count == 1)
            {
                await context.PostAsync($"Sure thing! Looks like {meetupResult.MatchingRsvps.First().member.name } is going!");
                context.Wait(MessageReceived);
                return;
            }

            // Has a number of matches.
            await context.PostAsync("We have a couple of people by that name actually, among the RSVP's for the event I have:");
            await context.PostAsync(
                MemberCarouselFactory.GetCarouselForRestaurants(context, meetupResult.MatchingRsvps.ToList()
             ));

            context.Wait(MessageReceived);
        }

        [LuisIntent("FindRaffleWinner")]
        public async Task HandleFindRaffleWinnerIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Yay! Ok here go's....");
            await context.PostAsync("And the winner is...");
            var winner = await _meetupService.PickRaffleWinnerAsync();
            await context.PostAsync(winner.member.name);
            await context.PostAsync("Congratulations!");
           
            context.Wait(MessageReceived);
        }
    }
}