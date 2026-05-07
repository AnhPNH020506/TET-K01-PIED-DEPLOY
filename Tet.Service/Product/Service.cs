using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Tet.Repository;
using Tet.Repository.Entity;

namespace Tet.Service.Product;

public class Service: IService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public Service(AppDbContext dbContext,  IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }

    public async Task<string> CreateProduct(Request.CreateProductRequest request)
    {
        var sellerId = _httpContext.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "SellerId")?.Value;
        
        var sellerIdGuid = Guid.Parse(sellerId!);
        var existingProductQuery =  _dbContext.Products.
            Where(x => x.Name.ToLower().Trim() ==(request.Name.ToLower().Trim()));
        bool isExist = await existingProductQuery.AnyAsync();
        if (isExist)
        {
            throw new Exception("Product already exists");
        }
        var existingSellerQuery =  _dbContext.Sellers.
            Where(x => x.Id == sellerIdGuid);
        bool isExistSeller = await existingSellerQuery.AnyAsync();
        if (!isExistSeller) throw new Exception("Seller does not exist");
        
        
        var product = new Repository.Entity.Product()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity,
            SellerId =sellerIdGuid
        };
        _dbContext.Add(product);
        var result = await _dbContext.SaveChangesAsync();
        if(request.CategoryIds != null && request.CategoryIds.Count > 0)
        {
            var productCateList = request.CategoryIds.Select(id => new ProductCategory()
            {
                CategoryId = id,
                ProductId = product.Id
            });
            
            // Same with above but using foreach loop
            // var productCateList1 = new List<ProductCategory>();
            // foreach (var id in request.CategoryIds)
            // {
            //     var productCate = new ProductCategory()
            //     {
            //         CategoryId = id,
            //         ProductId = product.Id
            //     };
            //     productCateList1.Add(productCate);
            // }
            
            _dbContext.AddRange(productCateList);
            await _dbContext.SaveChangesAsync();
        }
        
        if (result > 0) return "Add Product successfully";
            
        return "Add Product failed";
    }
   
}