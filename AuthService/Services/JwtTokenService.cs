using AuthService.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace AuthService.Services
{
    public class JwtTokenService
    {
        private IConfiguration _configuration;
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /*public AuthenticationToken? GenerateAuthToken(LoginUser loginModel)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecurityKey")));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var expirationTimeStamp = DateTime.Now.AddMinutes(5);
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Name, loginModel.username),
            new Claim("role", loginModel.role),
            new Claim("scope", string.Join(" ", loginModel.forumIDs))
        };
            var tokenOptions = new JwtSecurityToken(
                issuer: "https://localhost:5002",
                claims: claims,
                expires: expirationTimeStamp,
                signingCredentials: signingCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new AuthenticationToken();
        }*/

        public string GenerateAuthToken(LoginUser loginUser)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.id.ToString()),
                new Claim(ClaimTypes.Role, loginUser.role)
            };
            foreach (Guid id in loginUser.forumIDs)
            {
                claims.Add(new Claim("forums", id.ToString()));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims/*new Claim[]
            {
                new Claim(ClaimTypes.Name, loginUser.id.ToString()),
                new Claim(ClaimTypes.Role, loginUser.role),
                new Claim("forums", string.Join(" ", loginUser.forumIDs))
            }*/),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    //new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecurityKey"))),
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ThePerfectAudiomindSecurityKeyForJWT")),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
