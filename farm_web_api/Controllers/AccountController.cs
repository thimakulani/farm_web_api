using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using farm_web_api.Data;
using farm_web_api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace farm_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApiContext _context;
        private IConfiguration _config;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(ApiContext context, IConfiguration config, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _config = config;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this._context = context;
        }


        // GET: api/UserLogins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLogin>>> GetUserLogins()
        {
            return null;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PostUserRegister(UserRegister user)
        {
            if(user == null)
            {
                return BadRequest("All fields are required");
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.Phone,
                UserName = user.Username
            };
            var results = await userManager.CreateAsync(applicationUser, user.Password);
            if (results.Succeeded)
            {
                return Ok(GenerateToken(applicationUser));
            }
            else
            {
                return BadRequest(results.Errors);
            }
        }
        // PUT: api/UserLogins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/UserLogins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserLogin>> PostUserLogin(UserLogin user)
        {
            if (user == null)
            {
                return BadRequest("All fields are required");
            }
            var results = await signInManager.PasswordSignInAsync(user.Username.Trim(), user.Password.Trim(), false, false);
            if (results.Succeeded)
            {
                var user_data = await userManager.FindByEmailAsync(user.Username);
                var token = GenerateToken(user_data);
                return Ok(token);
            }
            else
            {
                return NotFound("Username Or Password is incorrect");
            }
        }
        private string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                //new Claim(ClaimTypes.Role, user.PhoneNumber),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
