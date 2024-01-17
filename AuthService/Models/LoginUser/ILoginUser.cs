using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Steeltoe.Discovery.Eureka;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AuthService.Interfaces
{
    public class ILoginUser
    {
        public ILoginUser(LoginUser user)
        {
            id = user.id.ToString();
            username = user.username;
            hashedPassword = user.hashedPassword; 
            role = user.role;
            forumIDs = "";
            for (int i = 0; i < user.forumIDs.Count; i++)
            {
                if (user.forumIDs.Count - 1 == i)
                    forumIDs += user.forumIDs[i].ToString();
                else
                    forumIDs += user.forumIDs[i].ToString() + ",";
            }
        }

        public ILoginUser() { }

        [Required, Key]
        public string id { get; set; }

        [Required]
        public string username { get; set; }

        [Required]
        public string hashedPassword { get; set; }

        [Required]
        public string role { get; set; }

        public string forumIDs { get; set; }
    }
}
