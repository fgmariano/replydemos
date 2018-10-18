using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using System;
using System.Threading.Tasks;

namespace Reply_LuisBot.Dialogs
{
    [LuisModel("<appKey>", "<SubscriptionKey>")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Não entendi o que você quis dizer, poderia repetir?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("filme")]
        public async Task Filme(IDialogContext context, LuisResult result)
        {
            context.Call(new MovieDialog(), AfterDialogAsync);
        }

        [LuisIntent("saudacoes")]
        public async Task Saudacoes(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá, sou o bot da reply especializado em serviços cognitivos da Microsoft. " +
                "Que tal me fazer uma pergunta?");
            context.Wait(MessageReceived);
        }

        [LuisIntent("CognitiveServices")]
        public async Task CognitiveServices(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Os Serviços Cognitivos da Microsoft são um leque de ferramentas baseadas em " +
                "Machine Learning feitas para ajudar o desenvolvedor a implementar tecnologias como inteligência " +
                "artificial e reconhecimento de texto em seus projetos de forma simples e rápida, através de uma " +
                "Rest API.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("reply")]
        public async Task Reply(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("A REPLY TEM COMO COMPROMISSO O CONTÍNUO PROCESSO DE PESQUISA, SELEÇÃO E PROMOÇÃO " +
                "DE SOLUÇÕES INOVADORAS CAPAZES DE APOIAR A CRIAÇÃO DE VALOR NAS DIVERSAS ORGANIZAÇÕES. " +
                "Veja mais detalhes no [https://www.reply.com](https://www.reply.com)");
            context.Wait(MessageReceived);
        }

        [LuisIntent("chatbot-funcionamento")]
        public async Task Chatbot_Funcionamento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("O Microsoft Bot Framework é uma ferramenta feita pela Microsoft " +
                "para facilitar o desenvolvimento de chatbots para diversas plataformas. Atualmente, " +
                "o framework suporta o desenvolvimento em C# e Node.JS.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("chatbot-entende")]
        public async Task Chatbot_Entende(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sim, esse bot usa uma inteligência artificial chamada LUIS, feita pela Microsoft, " +
                "para entender exatamente o que você está falando, experimente me perguntar o que é LUIS.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("luis")]
        public async Task Luis(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("O Language Understanding (LUIS) é um serviço cognitivo da Microsoft que combina " +
                "a facilidade do desenvolvimento do Bot Framework com o poder do Machine Learning para prover ao " +
                "desenvolvedor um meio de criar bots cada vez mais dinâmicos.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("luis-entende")]
        public async Task Luis_Entende(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sim, através do treinamento de intenções previamente cadastradas pelo desenvolvedor, " +
                "o LUIS consegue entender com exatidão qual a intenção do usuário com aquela frase e direcionar o bot " +
                "para a melhor resposta.");
            context.Wait(MessageReceived);
        }

        public async Task AfterDialogAsync(IDialogContext context, IAwaitable<object> argument)
        {
            context.Wait(MessageReceived);
        }
    }
}