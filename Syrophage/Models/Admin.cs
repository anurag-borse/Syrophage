using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string? Address { get; set; }

        public string? Contact { get; set; }

        public string? Name { get; set; }

        [NotMapped]
        public IFormFile? ProfileImage { get; set; }

        public string? ProfileImageUrl { get; set; }

    }
}
