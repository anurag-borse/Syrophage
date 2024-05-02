using System.ComponentModel.DataAnnotations;

namespace Syrophage.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter Email .")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        //[Required(ErrorMessage = "Please Enter Password .")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$", ErrorMessage = "Password Must be Strong")]
        public string Password { get; set; }
    }
}
