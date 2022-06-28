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
using System.Security.Cryptography;

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
        public Task<ActionResult<IEnumerable<UserLogin>>> GetUserLogins()
        {
            return null;
        }
        [AllowAnonymous]
        [HttpPost("register")]
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
                UserName = user.Username,
                RoleName = user.Role,
                
            };
            var results = await userManager.CreateAsync(applicationUser, user.Password);
            
            if (results.Succeeded)
            {
                var _user = await userManager.FindByEmailAsync(user.Email.Trim());
                await userManager.AddToRoleAsync(_user, user.Role);
                var response = new AuthResponse
                {
                    Token = GenerateToken(_user),
                    ApplicationUser = _user
                };
                return Ok(response);
            }
            else
            {
                string errors = "";
                foreach (var item in results.Errors)
                {
                    errors += item.Description +"\n";
                }
                throw new UnauthorizedAccessException(errors);
            }
        }
        // PUT: api/UserLogins/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // POST: api/UserLogins
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserLogin>> PostUserLogin(UserLogin user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var results = await signInManager.PasswordSignInAsync(user.Username.Trim(), user.Password.Trim(), false, false);
            if (results.Succeeded)
            {
                var user_data = await userManager.FindByEmailAsync(user.Username);
                //role = roleManager
                
                var token = GenerateToken(user_data);
                
                return Ok(token);
            }
            else
            {
                throw new UnauthorizedAccessException("Username Or Password is incorrect");
            }
        }

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(byteArray);

            return Convert.ToBase64String(byteArray);
           
        }

        private string GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                //new Claim(ClaimTypes.Role, user.PhoneNumber),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    public class AuthResponse
    {
        public string Token { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
