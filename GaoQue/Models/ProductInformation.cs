namespace GaoQue.Models
{
    public class ProductInformation
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
