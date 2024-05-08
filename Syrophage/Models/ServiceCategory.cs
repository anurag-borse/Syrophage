using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class ServiceCategory
    {
        [Key]
        public int Id { get; set; }

        public string ServiceCategoryName { get; set; }

        public string? ServiceCategoryDescription { get; set; }

        [NotMapped]
        [ValidateNever]
        public IFormFile? ServiceCategoryPicture { get; set; }


        [ValidateNever]
        public string? ServiceCategoryPictureUrl { get; set; }

    }
}
