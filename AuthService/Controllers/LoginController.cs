using Microsoft.AspNetCore.Mvc;
using AuthService.Data;
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Services;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using MassTransit;
using MassTransit.Transports;
using Audiomind.RabbitMQ.Moddels;
using Audiomind.RabbitMQ;
using System.Security.Claims;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ISqlLoginUser _sqlLoginUser;
        private readonly EncryptionService _encryption;
        private readonly JwtTokenService _tokenService;
        private readonly IMessageProducer _messagePublisher;
        private readonly IPublishEndpoint _publishEndpoint;
        public LoginController(ISqlLoginUser sqlLoginUser, EncryptionService encryption, JwtTokenService tokenService, IMessageProducer messageProducer, IPublishEndpoint publishEndpoint) 
        {
            _sqlLoginUser = sqlLoginUser;
            _encryption = encryption;
            _tokenService = tokenService;
            _messagePublisher = messageProducer;
            _publishEndpoint = publishEndpoint;

            //Debug.WriteLine(BCrypt.Net.BCrypt.EnhancedHashPassword("JustAPassword@!", 13));
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

            _messagePublisher.SendMessage(new CreateMessage() { id = createdUser.id.ToString(), displayName = createdUser.username }, _publishEndpoint);

            string token = _tokenService.GenerateAuthToken(createdUser);
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

        [HttpGet("{userId}", Name = "Read"), Authorize(Roles = "administrator")]
        public IActionResult GetUser(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest("UserId field can't be empty.");


            LoginUser foundUser = _sqlLoginUser.GetLoginUser(userId);
            if (foundUser == null) return NotFound("User doesn't exists!");

            return Ok(foundUser);
        }

        [HttpGet("all", Name = "ReadAll"), Authorize(Roles = "administrator")]
        public IActionResult GetAll()
        {
            List<LoginUser> foundUser = _sqlLoginUser.GetLoginUsers();
            if (foundUser == null || foundUser.Count == 0) return NotFound("No users exist in the database.");

            return Ok(foundUser);
        }

        [HttpPatch("", Name = "Update"), Authorize]
        public IActionResult Update(LoginUser user)
        {
            if (user.id.ToString() == null || user.id.ToString() == string.Empty) return BadRequest("UserId field can't be empty.");
            if (User.Claims.Contains(new Claim(ClaimTypes.Name, user.id.ToString())) || User.IsInRole("administrator"))
            {
                LoginUser existing = _sqlLoginUser.GetLoginUser(user.id);
                if (existing == null) return NotFound("Couldn't find account.");

                existing.username = user.username;
                if (user.hashedPassword != null) existing.fullHashedPassword = _encryption.Hash(user.hashedPassword);
                LoginUser success = _sqlLoginUser.UpdateLoginUser(existing);

                if (success == null) return Problem("Couldn' t update account", string.Empty, 500);
                return Ok("Account updated");
            }
            else return Unauthorized();
        }

        [HttpDelete("", Name = "Delete"),Authorize(Roles = "administrator")]
        public IActionResult DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest("UserId field can't be empty.");
            bool success = _sqlLoginUser.DeleteLoginUser(userId);
            _messagePublisher.SendMessage(new DeleteMessage() { id = userId.ToString() }, _publishEndpoint);

            if (!success) return Problem("Couldn't delete user", string.Empty, 500);

            return Ok("Account deleted!");
        }
    }
}
