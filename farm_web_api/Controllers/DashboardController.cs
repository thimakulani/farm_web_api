using farm_web_api.Data;
using farm_web_api.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace farm_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly ApiContext _context;
        private UserManager<ApplicationUser> _userManager;
        public DashboardController(ApiContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Get()
        {
            DashboardViewModel viewModel = new DashboardViewModel()
            {
                Customers = _userManager.Users.Where(x => x.RoleName == "Customer").ToList().Count,
                Drivers = _userManager.Users.Where(x => x.RoleName == "Driver").ToList().Count,
                Employees = _userManager.Users.Where(x => x.RoleName == "Employee").ToList().Count,
                Orders = _context.Orders.Where(x=>x.Status == 0).ToList().Count,
            };
            return Ok(viewModel);
        }
    }
}
