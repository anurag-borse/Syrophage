using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string? CategoryDescription { get; set; }


        [NotMapped]
        [ValidateNever]
        public IFormFile? CategoryPicture { get; set; }


        [ValidateNever]
        public string? CategoryPictureUrl { get; set; }


    }
}
