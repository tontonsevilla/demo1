using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AspDotNetCoreDemo.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
