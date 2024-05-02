using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }

        public string productname { get; set; }
        public string Description { get; set; }

        public string Category { get; set; }

        public string Company { get; set; }


        [ValidateNever]
        public string productImageUrl { get; set; }
    }
}
