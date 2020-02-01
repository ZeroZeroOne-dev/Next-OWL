using System;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Next_OWL.Models.Config;
using Next_OWL.Models.Output;
using Next_OWL.Services;
using Next_OWL.Versions.V1.Models.Input;

namespace Next_OWL.Versions.V1.Services
{
    public class OwlService : IOwlService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;

        public OwlService(OWLApiConfig owlApiConfig)
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(owlApiConfig.V1BaseUrl);

            this.jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<RequestResult> GetSchedule()
        {
            var request = await this.httpClient.GetAsync("/schedule?locale=en_US&season=2020");
            using var jsonStream = await request.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RequestResult>(jsonStream, jsonOptions);
        }

        public async Task<IOrderedEnumerable<Game>> GetFuture(int count = 10)
        {
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var owlSchedule = (await GetSchedule()).Data;

            var matches = owlSchedule.Stages
                        .SelectMany(s => s.Matches)
                        .Where(m => m.Competitors[0] != null && m.StartDateTS >= start)
                        .OrderBy(s => s.StartDate)
                        .Take(count)
                        .Select(m => new Game
                        {
                            TeamOne = new Team
                            {
                                Name = m.Competitors[0].Name,
                                Icon = m.Competitors[0].Icon
                            },
                            TeamTwo = new Team
                            {
                                Name = m.Competitors[1].Name,
                                Icon = m.Competitors[1].Icon
                            },
                            Date = m.StartDate
                        })
                        .OrderBy(g => g.Date);

            return matches;
        }

        public async Task<Game> GetNext()
        {
            var futureGames = await this.GetFuture(1);
            return futureGames.FirstOrDefault();
        }
    }
}