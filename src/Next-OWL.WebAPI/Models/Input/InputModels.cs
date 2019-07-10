using System;
using System.Collections.Generic;

namespace Next_OWL.Models.Input
{
    public class RequestResult
    {
        public RequestData Data { get; set; }
    }

    public class RequestData
    {
        public IEnumerable<Stage> Stages { get; set; }
    }

    public class Stage
    {
        public IEnumerable<Match> Matches { get; set; }
    }

    public class Match
    {
        public DateTime StartDate { get; set; }
        public double StartDateTS { get; set; }
        public Competitor[] Competitors { get; set; }
    }

    public class Competitor
    {
        public string Name { get; set; }
    }
}