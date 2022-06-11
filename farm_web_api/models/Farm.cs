using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace farm_web_api.models
{
    public class Farm
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Display(Name = "Id#")]
        public int Id { get; set; }
        public string Name { get; set; }

        //
        public int AddressId { get; set; }
        public Address Address { get; set; }

        public List<Customer> Customer { get; set; }
    }
}
