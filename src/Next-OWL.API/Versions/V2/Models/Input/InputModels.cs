using System.Collections.Generic;

namespace Next_OWL.Versions.V2.Models.Input
{
    public class RequestResult
    {
        public Content Content { get; set; }
    }

    public class Content
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
        public string Icon { get; set; }
    }
}