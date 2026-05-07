namespace Tet.Service.Seller;

public class SellerRequest
{
    public class CreateSellerRequest : Identity.Request.CreateUserRequest
    {
        
        
        public required string CompanyName { get; set; }
        public required string CompanyAddress { get; set; }
        public required string TaxCode { get; set; }
        
    }
}