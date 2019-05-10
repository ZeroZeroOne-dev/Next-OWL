using System;
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
            var request = await this.httpClient.GetAsync("/schedule");
            return await request.Content.ReadAsAsync<RequestResult>();
        }

        public async Task<NextGame> GetNextGame()
        {
            var now = DateTime.Now;
            var start = GetUtcStamp(now);

            var inAWeek = now.AddDays(7);
            var end = GetUtcStamp(inAWeek);


            var owlSchedule = (await GetSchedule()).Data;
            var week = owlSchedule.Stages
                        .SelectMany(s => s.Weeks)
                        .OrderBy(w => w.StartDate)
                        .FirstOrDefault(w => w.StartDate.IsBetween(start, end) || w.EndDate.IsBetween(start, end));

            if (week == null)
            {
                return null;
            }

            var match = week.Matches
                        .OrderBy(m => m.StartDateTS)
                        .FirstOrDefault(m => m.StartDateTS.IsBetween(start, end));

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

        private double GetUtcStamp(DateTime dateTime)
        {
            return dateTime.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }


    }

    public static class ExtensionMethods
    {
        public static bool IsBetween(this double testValue, double min, double max)
        {
            return testValue >= min && testValue <= max;
        }
    }

}