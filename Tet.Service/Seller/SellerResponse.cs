using Tet.Service.Base;
using Response = Tet.Service.User.Response;

namespace Tet.Service.Seller;

public class SellerResponse
{
    public class GetSellerResponse : User.Response.GetAllUserResponse
    {
        public string? TaxCode { get; set; }
        public string? CompanyName { get; set; } 
       
    }
    public class GetSellerByIdResponse : User.Response.GetUserResponse
    {
        public string? TaxCode { get; set; } 
        public string? CompanyName { get; set; } 
        public string? CompanyAddress { get; set; }
       
    }
    
}