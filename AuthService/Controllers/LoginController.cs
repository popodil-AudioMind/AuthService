using Microsoft.AspNetCore.Mvc;
using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISqlLoginUser _sqlLoginUser;
        private readonly EncryptionService _encryption;
        private readonly JwtTokenService _tokenService;
        public LoginController(ISqlLoginUser sqlLoginUser, EncryptionService encryption, JwtTokenService tokenService) 
        {
            _sqlLoginUser = sqlLoginUser;
            _encryption = encryption;
            _tokenService = tokenService;

            Debug.WriteLine(BCrypt.Net.BCrypt.EnhancedHashPassword("JustAPassword@!", 13));
        }

        [HttpPost("Create", Name = "Create")]
        public IActionResult Create(LoginUser user)
        {
            if (user == null) return BadRequest();
            else if (user.username == null || user.username == string.Empty) return BadRequest("Username can't be empty.");
            else if (user.hashedPassword == null || user.hashedPassword == string.Empty) return BadRequest("Password can't be empty.");

            user.hashedPassword = _encryption.Hash(user.hashedPassword);
            LoginUser createdUser = _sqlLoginUser.AddLoginUser(user);
            if (createdUser == null) return BadRequest("User already exists!");

            string token = _tokenService.GenerateAuthToken(createdUser);
            //TODO: Messaging
            return Created("User database", token);
        }

        [HttpPost("", Name = "Login")]
        public IActionResult Login(LoginUser user)
        {
            if (user == null) return BadRequest();
            else if (user.username == null || user.username == string.Empty) return BadRequest("Username can't be empty.");
            else if (user.hashedPassword == null || user.hashedPassword == string.Empty) return BadRequest("Password can't be empty.");

            LoginUser foundUser = _sqlLoginUser.GetLoginUser(user.id);
            bool success = _encryption.Verify(user.hashedPassword, foundUser.fullHashedPassword);
            //LoginUser loggedInUser = _sqlLoginUser.Login(user);
            if (success == false) return BadRequest("Username/password invalid.");

            string token = _tokenService.GenerateAuthToken(foundUser);
            return Ok(token);
        }

        [HttpGet("{email}", Name = "Read")]
        public IActionResult GetUser(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest("UserId field can't be empty.");


            LoginUser foundUser = _sqlLoginUser.GetLoginUser(userId);
            if (foundUser == null) return NotFound("User doesn't exists!");

            return Ok(foundUser);
        }

        [HttpGet("", Name = "ReadAll")]
        public IActionResult GetAll()
        {
            List<LoginUser> foundUser = _sqlLoginUser.GetLoginUsers();
            if (foundUser == null || foundUser.Count == 0) return NotFound("No users exist in the database.");

            return Ok(foundUser);
        }

        [HttpPatch("", Name = "Update")]
        public IActionResult Update()
        {
            return Ok("Accoutn updated");
        }

        [HttpDelete("", Name = "Delete"),Authorize(Roles = "administrator")]
        public IActionResult DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest("UserId field can't be empty.");
            bool success = _sqlLoginUser.DeleteLoginUser(userId);

            if (!success) return Problem("Couldn't delete user", string.Empty, 500);

            //TODO: Messaging
            return Ok("Account deleted!");
        }
    }
}
