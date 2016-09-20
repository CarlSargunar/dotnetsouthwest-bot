using DotNetSouthWest.Meetup.Model;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetSouthWest.Bot.CardFactories
{
    public class MemberCarouselFactory
    {
        public static IMessageActivity GetCarouselForRestaurants(IDialogContext context, List<Rsvp> rsvps)
        {
            var reply = context.MakeMessage();
            reply.Attachments = new List<Attachment>();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (var rsvp in rsvps)
            {
                var actions = new List<CardAction>
                {
                    new CardAction
                    {
                        Title = "Meetup Profile",
                        Value = $"https://www.meetup.com/dotnetsouthwest/members/{rsvp.member.id}/",
                        Type = ActionTypes.OpenUrl,
                    }
                };

                reply.Attachments.Add(
                     new HeroCard
                     {
                         Title = rsvp.member.name,
                         Subtitle = $"{rsvp.member.bio}",
                         Images = new List<CardImage>
                         {
                             new CardImage
                             {
                                 Url = rsvp.member.photo.photo_link,
                                 Alt = rsvp.member.name
                             }
                         },
                         Buttons = actions
                     }.ToAttachment()
                );
            }

            return reply;
        }
    }
}