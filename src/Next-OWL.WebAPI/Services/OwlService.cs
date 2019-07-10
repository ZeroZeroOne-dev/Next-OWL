using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Next_OWL.Models.Config;
using Next_OWL.Models.Input;
using Next_OWL.Models.Output;

namespace Next_OWL.Services
{
    public class OwlService
    {
        private readonly HttpClient httpClient;

        public OwlService(OWLApiConfig owlApiConfig)
        {
            this.httpClient = new HttpClient();
            this.httpClient.BaseAddress = new Uri(owlApiConfig.BaseUrl);
        }

        private async Task<RequestResult> GetSchedule()
        {
            var request = await this.httpClient.GetAsync("/schedule?expand=team.content&locale=en_US&season=2019&separateStagePlayoffsWeek=true");
            return await request.Content.ReadAsAsync<RequestResult>();
        }

        public async Task<IOrderedEnumerable<Game>> GetFutureGames()
        {
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var owlSchedule = (await GetSchedule()).Data;

            var matches = owlSchedule.Stages
                        .SelectMany(s => s.Matches)
                        .Where(m => m.Competitors[0] != null && m.StartDateTS >= start)
                        .Select(m => new Game
                        {
                            TeamOne = m.Competitors[0].Name,
                            TeamTwo = m.Competitors[1].Name,
                            Date = m.StartDate
                        })
                        .OrderBy(g => g.Date);

            return matches;
        }

        public async Task<Game> GetNextGame()
        {
            var futureGames = await this.GetFutureGames();
            var game = futureGames.FirstOrDefault();

            return game;
        }
    }
}