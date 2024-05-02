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

        public IFormFile? CategoryPicture { get; set; }

        public string? CategoryPictureUrl { get; set; }


    }
}
