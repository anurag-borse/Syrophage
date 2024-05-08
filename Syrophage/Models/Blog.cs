using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Blog
    {
        [Key]
        public int id { get; set; }

        public string Name { get; set; }

        public string email { get; set; }
        [ValidateNever]

        public int Like { get; set; }
        [ValidateNever]

        public int Comments { get; set; }
        [ValidateNever]

        public DateOnly date { get; set; }
        public string BlogDesc { get; set;}


        [ValidateNever]

        public string ImageUrl { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

    }
}
