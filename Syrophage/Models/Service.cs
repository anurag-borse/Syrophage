using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Service
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string servicename { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }

        [ValidateNever]
        public string productImageUrl { get; set; }
    }
}
