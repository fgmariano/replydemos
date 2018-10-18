using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reply_LuisBot.Dialogs
{
    [Serializable]
    public class OldDialog : IDialog<object>
    {
        private string subscriptionKey = "";
        private string region = "";
        private string appKey = "";
        private double scoreLuis = 0.50;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            using (var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);
                    var uri = string.Format("https://{0}.api.cognitive.microsoft.com/luis/v2.0/apps/{1}?q={2}", region, appKey, message.Text);
                    var response = await client.GetAsync(uri);
                    var myInstance = JsonConvert.DeserializeObject<LuisResult>(await response.Content.ReadAsStringAsync());
                    if (myInstance.topScoringIntent != null && myInstance.topScoringIntent.score >= scoreLuis)
                    {
                        await context.PostAsync(GetMensagem(myInstance.topScoringIntent.intent));
                    }
                }
                catch (Exception ex)
                {
                    await context.PostAsync(ex.Message);
                }
            }
        }

        private string GetMensagem(string intent)
        {
            switch (intent)
            {
                case "reply":
                    return "A REPLY tem como compromisso o contínuo processo de pesquisa, seleção e promoção " +
                        "de soluções inovadoras capazes de apoar a criação de valor nas diversas organizações. " +
                        "Veja mais detalhes no [https://www.reply.com](https://www.reply.com)";

                case "chatbot-funcionamento":
                    return "O Microsoft Bot Framework é uma ferramenta feita pela Microsoft " +
                        "para facilitar o desenvolvimento de chatbots para diversas plataformas. Atualmente, " +
                        "o framework suporta o desenvolvimento em C# e Node.JS.";

                case "saudacoes":
                    return "Olá, sou o bot da reply especializado em serviços cognitivos da Microsoft. " +
                        "Que tal me fazer uma pergunta?";

                case "CognitiveServices":
                    return "Os Serviços Cognitivos da Microsoft são um leque de ferramentas baseadas em " +
                        "Machine Learning feitas para ajudar o desenvolvedor a implementar tecnologias como inteligência " +
                        "artificial e reconhecimento de texto em seus projetos de forma simples e rápida, através de uma " +
                        "Rest API.";

                case "chatbot-entende":
                    return "Sim, esse bot usa uma inteligência artificial chamada LUIS, feita pela Microsoft, " +
                        "para entender exatamente sobre o que você está falando.";

                case "luis":
                    return "O Language Understanding (LUIS) é um serviço cognitivo da Microsoft que combina " +
                        "a facilidade do desenvolvimento do Bot Framework com o poder do Machine Learning para prover ao " +
                        "desenvolvedor um meio de criar bots cada vez mais dinâmicos.";

                case "luis-entende":
                    return "Sim, através do treinamento de intenções previamente cadastradas pelo desenvolvedor, " +
                        "o LUIS consegue entender com exatidão qual a intenção do usuário com aquela frase e direcionar o bot " +
                        "para a melhor resposta.";
                default:
                    return "Não entendi o que você quis dizer, poderia repetir?";
            }
        }
    }

    #region Luis.Ai
    public class TopScoringIntent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public double score { get; set; }
    }

    public class LuisResult
    {
        public string query { get; set; }
        public TopScoringIntent topScoringIntent { get; set; }
    }
    #endregion
}