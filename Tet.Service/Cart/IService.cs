namespace Tet.Service.Cart;

public interface IService
{
    public Task CreateCart();
    
    public Task AddProductToCart(Request.AddProductToCartRequest request);
    
    public Task RemoveProductFromCart(Request.RemoveProductFromCartRequest request);
    //public Task RemoveAllProductFromCart();
    public Task<List<Response.ProductResponse>> GetCart();
    

}