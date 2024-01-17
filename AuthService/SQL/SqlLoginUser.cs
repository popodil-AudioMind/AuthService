using AuthService.Context;
using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;

namespace AuthService.SQL
{
    public class SqlLoginUser : ISqlLoginUser
    {
        private readonly AuthDatabaseContext _authContext;
        public SqlLoginUser(AuthDatabaseContext userContext)
        {
            this._authContext = userContext;
        }
        public LoginUser AddLoginUser(LoginUser user)
        {
            ILoginUser? guidUser = _authContext.LoginUsers.FirstOrDefault(x => x.id == user.id.ToString());
            if (guidUser == null)
            {
                /*Guid id = Guid.NewGuid();
                while (existingUser.id == id)
                {
                    id = Guid.NewGuid();
                }
                existingUser.id = id;*/
                _authContext.LoginUsers.Add(new ILoginUser(user));
                _authContext.SaveChanges();
                return user;
            }
            return null;
        }

        public bool DeleteLoginUser(Guid id)
        {
            ILoginUser? existingUser = _authContext.LoginUsers.FirstOrDefault(x => x.id == id.ToString());
            if (existingUser != null)
            {
                _authContext.LoginUsers.Remove(existingUser);
                _authContext.SaveChanges();
                return true;
            }
            return false;
        }

        public LoginUser GetLoginUser(Guid id)
        {
            ILoginUser? existingUser = _authContext.LoginUsers.FirstOrDefault(x => x.id == id.ToString());
            if (existingUser != null)
            {
                return new LoginUser(existingUser);
            }
            return null;
        }

        public List<LoginUser> GetLoginUsers()
        {
            List<LoginUser> users = new List<LoginUser>();
            foreach (ILoginUser iuser in _authContext.LoginUsers.ToList())
            {
                users.Add(new LoginUser(iuser));
            }
            return users;
        }

        /*public LoginUser Login(LoginUser user)
        {
            ILoginUser? existingUser = _authContext.LoginUsers.FirstOrDefault(x => x.id == user.id.ToString());
            if (existingUser != null)
            {
                if (user.fullHashedPassword == existingUser.hashedPassword) return new LoginUser(existingUser);
            }
            return null;
        }*/

        public LoginUser UpdateLoginUser(LoginUser user)
        {
            ILoginUser? existingUser = _authContext.LoginUsers.FirstOrDefault(x => x.id == user.id.ToString());
            if (existingUser != null)
            {
                existingUser.hashedPassword = user.fullHashedPassword;

                _authContext.LoginUsers.Update(existingUser);
                _authContext.SaveChanges();
                return new LoginUser(existingUser);
            }
            return null;
        }
    }
}
