using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Reply_FlowBot.Dialogs;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Reply_FlowBot.Controllers
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        [ResponseType(typeof(void))]
        public virtual async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity update = activity;
                if (update.MembersAdded != null && update.MembersAdded.Count > 0)
                {
                    var client = new ConnectorClient(new Uri(activity.ServiceUrl), new MicrosoftAppCredentials());
                    foreach (var newMember in activity.MembersAdded)
                    {
                        if (!newMember.Name.Contains("Bot"))
                        {
                            Activity reply = activity.CreateReply("Olá, sou o bot da reply. Como posso lhe chamar?");
                            await client.Conversations.ReplyToActivityAsync(reply);
                        }
                    }
                }
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}
