using System;

namespace Next_OWL.Models.Output
{
    public class Game
    {
        public Team TeamOne { get; set; }
        public Team TeamTwo { get; set; }
        public DateTime Date { get; set; }
    }

    public class Team
    {
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}