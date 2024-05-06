using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Qua_Service
    {
        [Key]
        public int id { get; set; }

        public string item { get; set; }

        public string Quantity { get; set; }

        public string Rate { get; set; }
    }
}
