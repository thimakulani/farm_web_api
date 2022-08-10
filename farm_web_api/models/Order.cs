namespace farm_web_api.models
{
    public class Order
    {
        public string Id { get; set; }
        
        public List<OrderItems> OrderItems { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
