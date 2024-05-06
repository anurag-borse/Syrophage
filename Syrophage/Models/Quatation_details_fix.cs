using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Quatation_details_fix
    {

        [Key]
        public int id { get; set; }
        public string Cname { get; set; }
        public string Cemail { get; set; }
        public string CPhoneNo { get; set; }
        public string CAboutUs { get; set; }
        public string CMethodology { get; set; }

    }
}
