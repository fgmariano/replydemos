using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Reply_QnABot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string QnAAuthKey;
        private string QnAKnowledgebaseId;
        private string QnAEndpointHostName;

        public async Task StartAsync(IDialogContext context)
        {
            QnAAuthKey = ConfigurationManager.AppSettings["QnAAuthKey"];
            QnAKnowledgebaseId = ConfigurationManager.AppSettings["QnAKnowledgebaseId"];
            QnAEndpointHostName = ConfigurationManager.AppSettings["QnAEndpointHostName"];

            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (!string.IsNullOrEmpty(QnAAuthKey) && !string.IsNullOrEmpty(QnAKnowledgebaseId))
            {
                // Forward to the appropriate Dialog based on whether the endpoint hostname is present
                if (string.IsNullOrEmpty(QnAEndpointHostName))
                    await context.Forward(new BasicQnAMakerPreviewDialog(), AfterAnswerAsync, message, CancellationToken.None);
                else
                    await context.Forward(new BasicQnAMakerDialog(), AfterAnswerAsync, message, CancellationToken.None);
            }
            else
            {
                await context.PostAsync("Please set QnAKnowledgebaseId, QnAAuthKey and QnAEndpointHostName (if applicable) in App Settings.");
            }

        }

        private async Task AfterAnswerAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            context.Wait(MessageReceivedAsync);
        }
    }

    // Dialog for QnAMaker Preview service
    [Serializable]
    public class BasicQnAMakerPreviewDialog : QnAMakerDialog
    {
        public BasicQnAMakerPreviewDialog() : base(new QnAMakerService(
            new QnAMakerAttribute(ConfigurationManager.AppSettings["QnAAuthKey"],
                ConfigurationManager.AppSettings["QnAKnowledgebaseId"]
                , defaultMessage: "No good match in FAQ.", scoreThreshold: 0.5)))
        { }
    }

    // Dialog for QnAMaker GA service
    [Serializable]
    public class BasicQnAMakerDialog : QnAMakerDialog
    {
        public BasicQnAMakerDialog() : base(new QnAMakerService(
            new QnAMakerAttribute(ConfigurationManager.AppSettings["QnAAuthKey"],
                ConfigurationManager.AppSettings["QnAKnowledgebaseId"], "No good match in FAQ.", 0.5, 1,
                ConfigurationManager.AppSettings["QnAEndpointHostName"])))
        { }

    }
}