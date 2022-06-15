using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farm_web_api.models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Id#"), Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
