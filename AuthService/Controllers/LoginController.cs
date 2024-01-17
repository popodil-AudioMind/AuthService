using Microsoft.AspNetCore.Mvc;
using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Services;
using System.Diagnostics;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISqlLoginUser _sqlLoginUser;
        private readonly EncryptionService _encryption;
        public LoginController(ISqlLoginUser sqlLoginUser, EncryptionService encryption) 
        {
            _sqlLoginUser = sqlLoginUser;
            _encryption = encryption;

            Debug.WriteLine(BCrypt.Net.BCrypt.EnhancedHashPassword("JustAPassword@!", 13));
        }

        [HttpPost("Create", Name = "Create")]
        public IActionResult Create(LoginUser user)
        {
            if (user == null) return BadRequest();
            else if (user.username == null || user.username == string.Empty) return BadRequest("Username can't be empty.");
            else if (user.hashedPassword == null || user.hashedPassword == string.Empty) return BadRequest("Password can't be empty.");


            LoginUser createdUser = _sqlLoginUser.AddLoginUser(user);
            if (createdUser == null) return BadRequest("User already exists!");

            return Created("User database", createdUser);
        }

        [HttpPost("", Name = "Login")]
        public IActionResult Login(LoginUser user)
        {
            if (user == null) return BadRequest();
            else if (user.username == null || user.username == string.Empty) return BadRequest("Username can't be empty.");
            else if (user.hashedPassword == null || user.hashedPassword == string.Empty) return BadRequest("Password can't be empty.");

            //user.salt = _sqlLoginUser.GetSalt(user.id);
            user.fullHashedPassword = _encryption.Hash(user.hashedPassword);
            LoginUser loggedInUser = _sqlLoginUser.Login(user);
            if (loggedInUser == null) return BadRequest("Username/password invalid.");

            return Ok(loggedInUser);
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

        [HttpDelete("", Name = "Delete")]
        public IActionResult DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest("UserId field can't be empty.");
            bool success = _sqlLoginUser.DeleteLoginUser(userId);

            if (!success) return Problem("Couldn't delete user", string.Empty, 500);
            return Ok("Account deleted!");
        }
    }
}
