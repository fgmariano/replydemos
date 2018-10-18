using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace Reply_FlowBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private string name;

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            if (message.Text == "/start" || message.Text.ToLower() == "oi" ||
                message.Text.ToLower() == "olá" || message.Text.ToLower() == "ola" ||
                message.Text.ToLower() == "alô" || message.Text.ToLower() == "alo")
            {
                await context.PostAsync("Olá, sou o bot da reply. Como posso lhe chamar?");
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = message.Text;
                }

                PromptDialog.Choice(
                    context: context,
                    resume: FirstFlowAsync,
                    prompt: $"Olá {name}, já conhece a REPLY?",
                    retry: "Escolha uma das opções acima.",
                    options: new string[] { "Sim", "Não" },
                    attempts: 2,
                    promptStyle: PromptStyle.Auto);
            }
        }

        public async Task FirstFlowAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string option = "";

            try
            {
                option = await argument;
            }
            catch (Exception)
            {
                await context.PostAsync("Nenhuma opção selecionada, saindo...");
                EndFlowAsync(context, true);
                return;
            }

            switch (option.ToLower())
            {
                case "sim":
                    await context.PostAsync("Ótimo! Você pode sempre acessar [reply.com/br](https://reply.com/br) para saber as nossas últimas notícias!");
                    break;
                case "não":
                    await context.PostAsync("A REPLY TEM COMO COMPROMISSO O CONTÍNUO PROCESSO DE PESQUISA, SELEÇÃO E PROMOÇÃO DE SOLUÇÕES INOVADORAS CAPAZES DE APOIAR A CRIAÇÃO DE VALOR NAS DIVERSAS ORGANIZAÇÕES. Veja mais detalhes no [https://www.reply.com](https://www.reply.com)");
                    break;
                default:
                    await context.PostAsync("Nenhuma opção selecionada, saindo...");
                    break;
            }

            PromptDialog.Choice(
                context: context,
                resume: SecondFlowAsync,
                prompt: "Você conhece a plataforma de chatbot da Microsoft?",
                retry: "Escolha uma das opções acima.",
                options: new string[] { "Sim", "Não" },
                attempts: 2,
                promptStyle: PromptStyle.Auto);
        }

        public async Task SecondFlowAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string option = "";

            try
            {
                option = await argument;
            }
            catch (Exception)
            {
                await context.PostAsync("Nenhuma opção selecionada, saindo...");
                EndFlowAsync(context, true);
                return;
            }

            switch (option.ToLower())
            {
                case "sim":
                    await context.PostAsync("Ótimo! Você pode aproveitar e acessar o código-fonte desse bot em " +
                        "[https://github.com/fgmariano/chatbot-demos](https://github.com/fgmariano/chatbot-demos) para aprender mais.");
                    break;
                case "não":
                    await context.PostAsync("Acelere o seu desenvolvimento com a plataforma de ChatBot da Microsoft. Detalhes: " +
                        "[https://dev.botframework.com/](https://dev.botframework.com/)");
                    break;
                default:
                    await context.PostAsync("Nenhuma opção selecionada, saindo...");
                    break;
            }

            PromptDialog.Choice(
                context: context,
                resume: ThirdFlowAsync,
                prompt: "Você conhece os cognitive services da Microsoft?",
                retry: "Escolha uma das opções acima.",
                options: new string[] { "Sim", "Não" },
                attempts: 2,
                promptStyle: PromptStyle.Auto);
        }

        public async Task ThirdFlowAsync(IDialogContext context, IAwaitable<string> argument)
        {
            string option = "";

            try
            {
                option = await argument;
            }
            catch (Exception)
            {
                await context.PostAsync("Nenhuma opção selecionada, saindo...");
                EndFlowAsync(context, true);
                return;
            }

            switch (option.ToLower())
            {
                case "sim":
                    await context.PostAsync("Veja mais detalhes dos serviços cognitivos da Microsoft em " +
                        "[https://azure.microsoft.com/pt-br/services/cognitive-services/](https://azure.microsoft.com/pt-br/services/cognitive-services/)");
                    break;
                case "não":
                    await context.PostAsync("Use inteligência artificial para resolver problemas de negócios. Veja mais detalhes dos serviços cognitivos da " +
                        "Microsoft em [https://azure.microsoft.com/pt-br/services/cognitive-services/](https://azure.microsoft.com/pt-br/services/cognitive-services/)");
                    break;
                default:
                    break;
            }

            EndFlowAsync(context);
        }

        public async Task EndFlowAsync(IDialogContext context, bool hasError = false)
        {
            if (hasError)
            {
                PromptDialog.Choice(
                    context: context,
                    resume: FirstFlowAsync,
                    prompt: "Você já conhece a REPLY?",
                    retry: "Escolha uma das opções acima.",
                    options: new string[] { "Sim", "Não" },
                    attempts: 3,
                    promptStyle: PromptStyle.Auto);
            }
            else
            {
                await context.PostAsync("Acesse o código desse chatbot no GitHub em [https://github.com/fgmariano/replydemos](https://github.com/fgmariano/replydemos)");
                context.Wait(MessageReceivedAsync);
            }
        }
    }
}