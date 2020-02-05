using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthenticationController(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            this._config = config;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(AuthUser user)
        {
            user.UserName = user.UserName.ToLower();
            if(await _userService.UserExits(user.UserName))
            {
                return BadRequest("User already exists.");
            }
            User newuser = new User(){
                UserName = user.UserName
            };
            var userinfo= await _userService.RegisterUser(newuser, user.Password);
            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Authentication(AuthUser user){
            var userinfo= await _userService.Login(user.UserName.ToLower(), user.Password);
            if(userinfo == null)
                return Unauthorized();
                
            var claims =  new []
            {
                new Claim(ClaimTypes.NameIdentifier, userinfo.Id.ToString()),
                new Claim(ClaimTypes.Name, userinfo.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds=  new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

            var tokendescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenhandler =  new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokendescriptor);
            
            return Ok(new {
                token= tokenhandler.WriteToken(token)
            });
        }
    }
}