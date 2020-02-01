using System;
using System.Collections.Generic;
using System.Globalization;
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

        private static int GetCurrentPageNumber()
        {
            var current = DateTimeFormatInfo.CurrentInfo.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Tuesday);
            var page = current - 5;
            return page < 1 ? 1 : page;
        }

        private async Task<RequestResult> GetPage(int page)
        {
            var request = await this.httpClient.GetAsync($"/production/owl/paginator/schedule?stage=regular_season&page={page}&season=2020&locale=en-us");
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

            var currentEvents = currentTask.Result.Content.TableData.Events;
            var nextEvents = nextTask.Result.Content.TableData.Events;

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