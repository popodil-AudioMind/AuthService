using Audiomind.RabbitMQ.Moddels;
using AuthService.Models;
namespace AuthService.Data
{
    public interface ISqlLoginUser
    {
        LoginUser GetLoginUser(Guid userId);
        List<LoginUser> GetLoginUsers();
        LoginUser AddLoginUser(LoginUser user);
        bool DeleteLoginUser(Guid userId);
        LoginUser UpdateLoginUser(LoginUser user);
        /*LoginUser Login(LoginUser user);*/
        bool AddForumToUser(ForumMessage message);
    }
}
