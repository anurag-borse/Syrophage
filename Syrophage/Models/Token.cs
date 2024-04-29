using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class Token
    {
        [Key]
        public int Id { get; set; }
        public string? RequestId { get; set; }

        public string RequestQuery { get; set; }


        public string RegId { get; set; }  
        public string Name { get; set; }

        public DateTime? Date { get; set; }

        [NotMapped]

        public IFormFile? Attachment1 { get; set; }

        public string? Attachment1Url { get; set; }
        [NotMapped]

        public IFormFile? Attachment2 { get; set; }

        public string? Attachment2Url { get; set; }
        public string? Status { get; set; }
    }
}
