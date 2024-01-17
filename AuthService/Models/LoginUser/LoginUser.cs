using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using AuthService.Interfaces;

namespace AuthService.Models
{
    public class LoginUser
    {
        public LoginUser(ILoginUser user)
        {
            id = Guid.Parse(user.id);
            username = user.username;
            hashedPassword = user.hashedPassword;
        }

        public LoginUser() { }

        [Key]
        [Required]
        public Guid id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string hashedPassword { get; set; }

        public string salt { get; set; }
        public string fullHashedPassword { get; set; }
    }
}
