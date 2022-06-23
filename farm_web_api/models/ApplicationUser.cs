using Microsoft.AspNetCore.Identity;

namespace farm_web_api.models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
