using System;

namespace AnimeWebApplication.Models
{
    public class AnimeItem
    {
        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public int SeriesCount { get; set; }
        public string PosterPath { get; set; }
    }
}