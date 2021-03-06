﻿using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Reply_QnABot.Dialogs;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Reply_QnABot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
