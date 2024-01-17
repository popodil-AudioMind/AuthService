using AuthService.Models;
namespace AuthService.Data
{
    public interface ISqlLoginUser
    {
        LoginUser GetLoginUser(Guid email);
        List<LoginUser> GetLoginUsers();
        LoginUser AddLoginUser(LoginUser user);
        bool DeleteLoginUser(Guid Email);
        LoginUser UpdateLoginUser(LoginUser user);
        /*LoginUser Login(LoginUser user);*/
    }
}
