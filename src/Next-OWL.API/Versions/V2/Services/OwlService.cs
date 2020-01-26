using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Next_OWL.Models.Config;
using Next_OWL.Models.Output;
using Next_OWL.Services;
using Next_OWL.Versions.V2.Models.Input;

namespace Next_OWL.Versions.V2.Services
{

    public class OwlService : IOwlService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;

        public OwlService(OWLApiConfig owlApiConfig)
        {
            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(owlApiConfig.V2BaseUrl)
            };
            this.httpClient.DefaultRequestHeaders.Add("referer", "https://overwatchleague.com/en-us/schedule");


            this.jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<RequestResult> GetSchedule()
        {
            var request = await this.httpClient.GetAsync("/production/owl/paginator/schedule?stage=regular_season&page=1&season=2020&locale=en-us");
            using var jsonStream = await request.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RequestResult>(jsonStream, jsonOptions);
        }

        public async Task<IOrderedEnumerable<Game>> GetFutureGames()
        {
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var owlSchedule = (await GetSchedule());

            var matches = owlSchedule.Content.TableData.Events
                        .SelectMany(s => s.Matches)
                        .Where(m => m.Competitors[0] != null && m.StartDate >= start)
                        .Select(m => new Game
                        {
                            TeamOne = new Team
                            {
                                Name = m.Competitors[0].Name,
                                ShortName = m.Competitors[0].AbbreviatedName,
                                Icon = m.Competitors[0].Icon
                            },
                            TeamTwo = new Team
                            {
                                Name = m.Competitors[1].Name,
                                ShortName = m.Competitors[1].AbbreviatedName,
                                Icon = m.Competitors[1].Icon
                            },
                            Date = DateTimeOffset.FromUnixTimeMilliseconds(m.StartDate).UtcDateTime
                        })
                        .OrderBy(g => g.Date);

            return matches;
        }

        public async Task<Game> GetNextGame()
        {
            var futureGames = await this.GetFutureGames();
            return futureGames.FirstOrDefault();
        }
    }
}