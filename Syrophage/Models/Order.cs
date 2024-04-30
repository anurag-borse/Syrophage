using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class Order
    {
        [Key]
       public int id {get;set;}


        [ForeignKey("User")]
        [ValidateNever]
        public int UserId { get; set; }

        public string username { get; set; }


        [NotMapped]
        [ValidateNever]
        public Users User { get; set; }


        public string ProductName { get; set; }

        [ValidateNever]
        public string ProductImageurl { get; set; }

        public int quantity { get; set; }
        public string Status { get; set; }

        [ValidateNever]
        public DateTime date { get; set; }

    }
}
