using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnimeWebApplication.Models
{
    public class Profile
    {
        public Guid Id { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
    }
}