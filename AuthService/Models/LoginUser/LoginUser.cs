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
            fullHashedPassword = user.hashedPassword;
            role = user.role;
            forumIDs = new List<Guid>();
            List<string> tempForumIDs = user.forumIDs.Split(',').ToList();
            foreach (string id in tempForumIDs)
            {
                forumIDs.Add(Guid.Parse(id));
            }
        }

        public LoginUser() { }

        [Key]
        [Required]
        public Guid id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string hashedPassword { get; set; }

        public string fullHashedPassword { get; set; }

        [Required]
        public string role { get; set; }

        [Required]
        public List<Guid> forumIDs { get; set; }
    }
}
