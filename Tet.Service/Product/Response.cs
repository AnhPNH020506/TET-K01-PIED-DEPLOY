namespace Tet.Service.Product;

public class Response
{
    public class GetProductResponse
    {
       public Guid Id { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public decimal Price { get; set; }
       public required int Quantity { get; set; }
       public required Guid SellerId { get; set; }
    }
}