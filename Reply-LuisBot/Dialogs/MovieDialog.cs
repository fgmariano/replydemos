using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reply_LuisBot.Dialogs
{
    [Serializable]
    public class MovieDialog : IDialog
    {
        private string request = "http://www.omdbapi.com/?apikey=&";
        private string imdb_uri = "https://www.imdb.com/title/";

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Qual filme você quer procurar?");
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;

            dynamic json;
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(request + "t=" + message.Text);
                json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            }

            if ((bool)json.Response)
            {
                var heroCard = new HeroCard()
                {
                    Title = json.Title,
                    Subtitle = json.Director + " - " + json.Year,
                    Text = json.Plot,
                    Images = new List<CardImage> { new CardImage((string)json.Poster) },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver no iMDB", value: imdb_uri + json.imdbID) }
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(reply);
                context.Done<object>(null);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(request + "s=" + message.Text);
                    json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                }

                if ((bool)json.Response)
                {
                    List<string> movies = new List<string>();
                    foreach (var item in json.Search)
                    {
                        movies.Add($"{item.Title} ({item.Year})");
                    }
                    movies.Add("Pesquisar novamente");


                    PromptDialog.Choice(
                        context,
                        SearchPromptAsync,
                        movies,
                        "Encontrei os seguintes filmes, escolha um pra abrir os detalhes dele ou clique em 'Pesquisar novamente' pra fazer outra busca",
                        "Escolha uma das opções acima", 2, PromptStyle.Auto);
                }
                else
                {
                    await context.PostAsync("I couldn't find any movie with this name, try another one.");
                    context.Wait(MessageReceivedAsync);
                }
            }
        }

        private async Task SearchPromptAsync(IDialogContext context, IAwaitable<string> argument)
        {
            var message = await argument;

            if (message.ToUpper() == "PESQUISAR NOVAMENTE")
            {
                await context.PostAsync("Qual filme você deseja procurar?");
                context.Wait(MessageReceivedAsync);
            }
            else
            {
                dynamic json;
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(request + "t=" + message);
                    json = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
                }

                if ((bool)json.Response)
                {
                    var heroCard = new HeroCard()
                    {
                        Title = json.Title,
                        Subtitle = json.Director + " - " + json.Year,
                        Text = json.Plot,
                        Images = new List<CardImage> { new CardImage((string)json.Poster) },
                        Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver no iMDB", value: imdb_uri + json.imdbID) }
                    };

                    var reply = context.MakeMessage();
                    reply.Attachments.Add(heroCard.ToAttachment());
                    await context.PostAsync(reply);
                    context.Done<object>(null);
                }
                else
                {
                    await context.PostAsync("Não foi possível fazer a busca, tente novamente mais tarde");
                    context.Wait(MessageReceivedAsync);
                }
            }
        }
    }
}