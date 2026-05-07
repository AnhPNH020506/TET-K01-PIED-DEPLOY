using Microsoft.EntityFrameworkCore;
using Tet.Repository;

namespace Tet.Service.User;

public   class Service : IService
{
    private readonly AppDbContext _dbContext;

    public Service(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Base.Response.PageResult<Response.GetUserResponse>> GetUsers(
        string? searchTerm, int pageSize, int pageIndex)
    {
        var query = _dbContext.Users.Where(x => true);

        if (searchTerm != null)
        {
            query = query.Where(x =>
                x.FirstName.Contains(searchTerm) ||
                x.LastName.Contains(searchTerm) ||
                x.Email.Contains(searchTerm));
        }

        query = query.OrderBy(x => x.Email);

        query = query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var selectedQuery = query
            .Select(x => new Response.GetUserResponse()
            {
                UserId = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImageUrl = x.ImageUrl,
                // phoneNumber = x.PhoneNumber,
                // address = x.Address,
                Role = x.Role,
            });

        var listResult = await selectedQuery.ToListAsync();
        var totalItems =listResult.Count();

        var reuslt = new Base.Response.PageResult<Response.GetUserResponse>()
        {
            Items = listResult,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalItems = totalItems,

        };
        return reuslt;

    }

    public async Task<Response.GetUserResponse> GetUserById(Guid id)
    {
        var query = _dbContext.Users.Where(x => x.Id == id);

        
        var selectedQuery = query
            .Select(x => new Response.GetUserResponse()
            {
                UserId = x.Id,
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImageUrl = x.ImageUrl,
                // phoneNumber = x.PhoneNumber,
                // address = x.Address,
                Role = x.Role,
            });
        var result = await selectedQuery.FirstOrDefaultAsync();
        
        return result;

    }
    //Get all Category(k phân trang, sort  theo bag chữ cai của Name
        //Map ra response như sau(Id,name)
    //Get all children Category va By Category Id,(k phan trang, sort theo bang chữ cái của Name)
        //Map ra response như sau(Id,name)
    //Get all Seller tôn tại trong hẹ thông (Phan trang, sort theo bang chu cai, cho tim kiem theo tên)
        //(Email, FirstName, LastName, ImageUrl, TaxCode, ComapanyName)
    //Get detail Seller By Id
        //Map ra response như sau(Email, FirstName, LastName, ImageUrl, PhoneNumber,
            //Address, DOB,TaxCode, CompanyNmae, CompanyAddress)
}