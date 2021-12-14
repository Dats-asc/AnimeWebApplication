using System;

namespace AnimeWebApplication.Models
{
    public class Anime
    {
        public Guid AnimeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}