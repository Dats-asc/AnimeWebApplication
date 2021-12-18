using System;
using Microsoft.AspNetCore.Http;

namespace AnimeWebApplication.Models
{
    public class UserPhoto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}