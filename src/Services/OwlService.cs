using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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

        public async Task<NextGame> GetNextGame()
        {
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var owlSchedule = (await GetSchedule()).Data;

            var match = owlSchedule.Stages
                        .SelectMany(s => s.Matches)
                        .Where(m => m.Competitors[0] != null)
                        .OrderBy(m => m.StartDateTS)
                        .FirstOrDefault(m => m.StartDateTS >= start);

            if (match == null)
            {
                return null;
            }

            return new NextGame
            {
                TeamOne = match.Competitors[0].Name,
                TeamTwo = match.Competitors[1].Name,
                Date = match.StartDate
            };

        }
    }
}