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
            return await request.Content.ReadAsAsync<JObject>();
        }

        public async Task<NextGame> GetNextGame()
        {
            var now = DateTime.Now;
            var start = GetUtcStamp(now);

            var inAWeek = now.AddDays(7);
            var end = GetUtcStamp(inAWeek);


            var owlSchedule = (await GetSchedule()).data;

            dynamic currentWeek = null;

            foreach (dynamic stage in owlSchedule.stages)
            {
                foreach (dynamic week in stage.weeks)
                {
                    var stageWeekStart = (double)week.startDate;
                    var stageEndWeek = (double)week.endDate;

                    if (stageWeekStart.IsBetween(start, end) || stageEndWeek.IsBetween(start, end))
                    {
                        currentWeek = week;
                        break; ;
                    }
                }
            }

            if (currentWeek == null)
            {
                return null;
            }

            foreach (dynamic match in currentWeek.matches)
            {
                var matchStart = (double)match.startDateTS;
                if (matchStart.IsBetween(start, end))
                {
                    return new NextGame
                    {
                        TeamOne = match.competitors[0].name,
                        TeamTwo = match.competitors[1].name,
                        Date = DateTime.Parse(match.startDate)
                    };
                }
            }

            return null;
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