using farm_web_api.Data;
using farm_web_api.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace farm_web_api.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsCategoryController : ControllerBase
    {
        private readonly ApiContext _context;

        public ProductsCategoryController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IEnumerable<Products> Get(int id)
        {
            var data = _context.Products.Where(x => x.CategoryId == id);
            if (data == null)
            {
                NotFound($"Data not found for category id {id}");
            }
            return data;
        }
    }
}
