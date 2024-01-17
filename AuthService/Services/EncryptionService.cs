using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class EncryptionService
    {
        public string Hash(string password)
        {
            string hashedPass = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 12);
            return hashedPass;
        }

        public bool Verify(string password, string hashedPassword)
        {
            bool correct = BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
            return correct;
        }
    }
}
