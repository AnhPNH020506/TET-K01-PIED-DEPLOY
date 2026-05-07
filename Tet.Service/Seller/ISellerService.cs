namespace Tet.Service.Seller;

public interface ISellerService
{
    public Task<Base.Response.PageResult<SellerResponse.GetSellerResponse>> GetSellers(
        string? searchTerm, int pageSize, int pageIndex);
    public Task<SellerResponse.GetSellerByIdResponse?> GetSellerById(Guid id);
    public Task<string>CreateSeller(SellerRequest.CreateSellerRequest request);
    
}