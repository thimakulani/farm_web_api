namespace farm_web_api.models
{
    public class OrderItems
    {
        public int Id { get; set; }
        public Products Products { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
