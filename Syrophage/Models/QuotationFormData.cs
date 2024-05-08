using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Syrophage.Models
{
    public class QuotationFormData
    {
        [Key]
        public int id { get; set; }
        public string QuotationBy { get; set; }
        public string PreparedBy { get; set; }
        public string Role { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string PreparedFor { get; set; }
        public string ContactTo { get; set; }
        public string EmailTo { get; set; }
        public string AboutUs { get; set; }
        public string Methodology { get; set; }
        public string Expectation { get; set; }
        public List<ServiceData> Services { get; set; }
        public string Term1 { get; set; }
        public string Term2 { get; set; }
        public string Term3 { get; set; }

    }

    public class ServiceData
    {
        [Key]
        public int Id { get; set; }
        public string Service { get; set; }
        public string Description { get; set; }
        public string OneTimeCharges { get; set; }
        public string HalfYearlyCharges { get; set; }
        public string AnnualCharges { get; set; }

        public int QuotationFormDataId { get; set; }
        public QuotationFormData QuotationFormData { get; set; }
    }

}




