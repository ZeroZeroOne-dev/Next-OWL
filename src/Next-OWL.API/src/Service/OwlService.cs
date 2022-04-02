using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Next_OWL.Models.Config;
using Next_OWL.Models.Output;
using Next_OWL.Service.Models.Input;

namespace Next_OWL.Service
{

    public class OwlService : IOwlService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOptions;
        private readonly OWLApiConfig owlApiConfig;

        public OwlService(OWLApiConfig owlApiConfig)
        {
            this.owlApiConfig = owlApiConfig;

            this.httpClient = new HttpClient
            {
                BaseAddress = new Uri(owlApiConfig.BaseUrl)
            };
            this.httpClient.DefaultRequestHeaders.Add("referer", "https://overwatchleague.com");
            this.httpClient.DefaultRequestHeaders.Add("x-origin", "overwatchleague.com");

            this.jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private int GetCurrentPageNumber()
        {
            var current = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Tuesday);
            var page = current + owlApiConfig.WeekOffset;
            return page < 1 ? 1 : page;
        }

        private async Task<RequestResult> GetPage(int page)
        {
            var request = await this.httpClient.GetAsync($"/production/v2/content-types/schedule/blt78de204ce428f00c/week/{page}");
            using var jsonStream = await request.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<RequestResult>(jsonStream, jsonOptions);
        }

        private async Task<IEnumerable<Event>> GetSchedule()
        {
            var currentPage = GetCurrentPageNumber();
            var nextPage = currentPage + 1;

            var currentTask = GetPage(currentPage);
            var nextTask = GetPage(nextPage);

            await Task.WhenAll(new Task[] { currentTask, nextTask });

            var currentEvents = currentTask.Result.Data?.TableData?.Events ?? Enumerable.Empty<Event>() ;
            var nextEvents = nextTask.Result.Data?.TableData?.Events ?? Enumerable.Empty<Event>();

            return currentEvents.Concat(nextEvents);
        }

        public async Task<IOrderedEnumerable<Game>> GetFuture(int count = 10)
        {
            var start = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var events = (await GetSchedule());

            var matches = events
                        .SelectMany(s => s.Matches)
                        .Where(m => m.Competitors[0] != null && m.StartDate >= start)
                        .OrderBy(s => s.StartDate)
                        .Take(count)
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

        public async Task<Game> GetNext()
        {
            var futureGames = await this.GetFuture(1);
            return futureGames.FirstOrDefault();
        }
    }
}