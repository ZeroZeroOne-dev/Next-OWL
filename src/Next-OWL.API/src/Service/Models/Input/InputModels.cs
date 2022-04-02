using System.Collections.Generic;

namespace Next_OWL.Service.Models.Input
{
    public class RequestResult
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public TableData TableData { get; set; }
    }

    public class TableData
    {
        public IEnumerable<Event> Events { get; set; }
    }

    public class Event
    {
        public IEnumerable<Match> Matches { get; set; }
    }

    public class Match
    {
        public long StartDate { get; set; }
        public Competitor[] Competitors { get; set; }
    }

    public class Competitor
    {
        public string Name { get; set; }
        public string AbbreviatedName { get; set; }
        public string Icon { get; set; }
    }
}