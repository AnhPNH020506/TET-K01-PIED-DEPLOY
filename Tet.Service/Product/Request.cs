namespace Tet.Service.Product;

public class Request
{
    public class CreateProductRequest
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public required int Quantity { get; set; }
        public List<Guid>? CategoryIds { get; set; }
        //public required Guid SellerId { get; set; }
    }
}