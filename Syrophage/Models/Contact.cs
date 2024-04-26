using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Contact
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string Email { get; set; }
        public string phone { get; set; }

        public string message { get; set; }
    }
}
