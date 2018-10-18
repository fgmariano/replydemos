using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reply_FlowBot.Dialogs
{
    [Serializable]
    public class CardsDialog : IDialog<object>
    {
        #region JSON
        private string imdb_uri = "https://www.imdb.com/title/";

        private string json = @"
{
    ""Title"": ""Primer"",
    ""Year"": ""2004"",
    ""Rated"": ""PG-13"",
    ""Released"": ""27 May 2005"",
    ""Runtime"": ""77 min"",
    ""Genre"": ""Drama, Sci-Fi, Thriller"",
    ""Director"": ""Shane Carruth"",
    ""Writer"": ""Shane Carruth"",
    ""Actors"": ""Shane Carruth, David Sullivan, Casey Gooden, Anand Upadhyaya"",
    ""Plot"": ""Four friends/fledgling entrepreneurs, knowing that there's something bigger and more innovative than the different error-checking devices they've built, wrestle over their new invention."",
    ""Language"": ""English, French"",
    ""Country"": ""USA"",
    ""Awards"": ""3 wins & 7 nominations."",
    ""Poster"": ""https://m.media-amazon.com/images/M/MV5BNjc3OWVjMTItYjc0Yi00NmFlLTk2YTgtYmU0MzcxMjBkNTYxXkEyXkFqcGdeQXVyMTQxNzMzNDI@._V1_SX300.jpg"",
    ""Ratings"": [
        {
            ""Source"": ""Internet Movie Database"",
            ""Value"": ""6.9/10""
        },
        {
            ""Source"": ""Rotten Tomatoes"",
            ""Value"": ""72%""
        },
        {
            ""Source"": ""Metacritic"",
            ""Value"": ""68/100""
        }
    ],
    ""Metascore"": ""68"",
    ""imdbRating"": ""6.9"",
    ""imdbVotes"": ""86,178"",
    ""imdbID"": ""tt0390384"",
    ""Type"": ""movie"",
    ""DVD"": ""05 Apr 2005"",
    ""BoxOffice"": ""$392,420"",
    ""Production"": ""ThinkFilm"",
    ""Website"": ""http://primermovie.com/"",
    ""Response"": ""True""
}";
        #endregion

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        public async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            dynamic movie = JsonConvert.DeserializeObject(json);

            if (message.Text == "hero")
            {
                HeroCard heroCard = new HeroCard()
                {
                    Title = movie.Title,
                    Subtitle = movie.Director + " - " + movie.Year,
                    Text = movie.Plot,
                    Images = new List<CardImage> { new CardImage((string)movie.Poster) },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver no iMDB", value: imdb_uri + movie.imdbID) }
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(heroCard.ToAttachment());
                await context.PostAsync(reply);
            }
            else if (message.Text == "thumbnail")
            {
                ThumbnailCard thumbnailCard = new ThumbnailCard()
                {
                    Title = movie.Title,
                    Subtitle = movie.Director + " - " + movie.Year,
                    Text = movie.Plot,
                    Images = new List<CardImage> { new CardImage((string)movie.Poster) },
                    Buttons = new List<CardAction> { new CardAction(ActionTypes.OpenUrl, "Ver no iMDB", value: imdb_uri + movie.imdbID) }
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(thumbnailCard.ToAttachment());
                await context.PostAsync(reply);
            }
            else if (message.Text == "audio")
            {
                AudioCard card = new AudioCard()
                {
                    Title = "Developers",
                    Subtitle = "Developers, developers, developers.",
                    Text = "Developers...",
                    Image = new ThumbnailUrl((string)movie.Poster),
                    Media = new List<MediaUrl> { new MediaUrl(@"C:\Users\f.mariano\Desktop\win95.mp4") },
                    Autoloop = false,
                    Autostart = false
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(card.ToAttachment());
                await context.PostAsync(reply);
            }
            else if (message.Text == "animation")
            {
                AnimationCard card = new AnimationCard()
                {
                    Title = "Title",
                    Subtitle = "Subtitle",
                    Text = "Text",
                    Media = new List<MediaUrl> { new MediaUrl(@"C:\Users\f.mariano\Desktop\win95.mp4") },
                    Autoloop = true,
                    Autostart = true
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(card.ToAttachment());
                await context.PostAsync(reply);
            }
            else if (message.Text == "signin")
            {
                CardAction button = new CardAction()
                {
                    Value = "https://reply.com",
                    Type = "signin",
                    Title = "Login"
                };
                SigninCard card = new SigninCard()
                {
                    Text = "Faça seu login",
                    Buttons = new List<CardAction>() { button }
                };

                var reply = context.MakeMessage();
                reply.Attachments.Add(card.ToAttachment());
                await context.PostAsync(reply);
            }
        }
    }
}