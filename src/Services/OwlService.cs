using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Next_OWL.Models;

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

        private async Task<dynamic> GetSchedule()
        {
            var request = await this.httpClient.GetAsync("/schedule");
            return request.Content.ReadAsAsync<JObject>();
        }

        public async Task<NextGame> GetNextGame()
        {
            return new NextGame
            {
                TeamOne = "a team",
                TeamTwo = "a second team",
                Date = DateTime.Now
            };

            var owlSchedule = await GetSchedule();
            foreach (dynamic stage in owlSchedule.stages)
            {
                foreach (dynamic week in stage.weeks)
                {

                }
            }
        }
    }

}