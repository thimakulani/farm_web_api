using farm_web_api.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace farm_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post(UserLogin data)
        {

            HttpClient client = new HttpClient();
            

            var userLogin = Newtonsoft.Json.JsonConvert.DeserializeObject<UserLogin>("");
            var user = Authanticate(data);
            if (user != null)
            {
                string token = GenerateToken(user);
                return Ok(token);
            }
            return BadRequest();
        }

        private string GenerateToken(Employee user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, "Employee")
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static Employee Authanticate(UserLogin userLogin)
        {
            Employee emp = new Employee()
            {
                LastName = "t",
                FirstName = "n",
                Email = "em",
                Phone = "p",

            };
            return emp;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("wow");
        }
    }
}
