using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class ServiceCategory
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string? CategoryDescription { get; set; }

        [ValidateNever]
        public string CategoryPictureUrl { get; set; }
    }
}
