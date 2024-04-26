using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models
{
    public class Users
    {

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Name .")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Please Enter Phone No .")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number should be exactly 10 digits.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number must be numeric.")]

        public string Phone { get; set; }


        [Required(ErrorMessage = "Please Enter Email .")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Please Enter Password .")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password Must be Strong")]
        public string Password { get; set; }

        public string? ConfirmPassword { get; set; }

        public bool? IsActivated { get; set; }
    }
}
