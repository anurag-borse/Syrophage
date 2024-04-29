using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Newsletter
    {
        [Key]
        public int id { get; set; }

        public string email { get; set; }
    }
}
