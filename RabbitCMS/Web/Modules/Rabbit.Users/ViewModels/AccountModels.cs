using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.Users.ViewModels
{
    public sealed class SignInViewModel
    {
        [DisplayName("账号")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }
}