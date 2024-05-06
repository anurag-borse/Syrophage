using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string productname { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Company { get; set; }

        [ValidateNever]
        public string productImageUrl { get; set; }
    }
}
