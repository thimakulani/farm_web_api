using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farm_web_api.models
{
    public class Products
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Id#")]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // 
    }
}
