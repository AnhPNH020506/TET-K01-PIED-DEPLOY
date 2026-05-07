using Microsoft.EntityFrameworkCore;
using Tet.Repository;
using Tet.Service.Base;
using Tet.Service.MailService;

namespace Tet.Service.Seller;

public class SellerService : ISellerService
{
    private readonly AppDbContext _dbContext;
    private readonly MailService.IService _mailService;

    public SellerService(AppDbContext dbContext,  MailService.IService mailService)
    {
        _dbContext = dbContext;
        _mailService = mailService;
    }

    public async Task<Response.PageResult<SellerResponse.GetSellerResponse>> GetSellers(string? searchTerm,
        int pageSize, int pageIndex)
    {
        var query = _dbContext.Sellers
            .Where(x => true);
        if (searchTerm != null)
        {
            query = query.Where(x =>
                x.User.Email.Contains(searchTerm) ||
                x.User.FirstName.Contains(searchTerm) ||
                x.User.LastName.Contains(searchTerm));
        }
        //đi tu bang seller r join qua user neu ngc lai se ton tg vì phải lập qa nhieu thg de tìm role ="Seller"
        //Khi filter theo bang User de dan den viec role = null, Dung dau ! thể hien đã biết

        query = query.OrderBy(x => x.User.Email);
        query = query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);
        var selectQuery = query
            .Select(x => new SellerResponse.GetSellerResponse()
            {
                Id = x.User.Id,
                Email = x.User.Email,
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                ImageUrl = x.User.ImageUrl,
                CompanyName = x.CompanyName,
                TaxCode = x.TaxCode
            });
        var listResult = await selectQuery.ToListAsync();
        var totalItems = listResult.Count();

        var result = new Base.Response.PageResult<SellerResponse.GetSellerResponse>()
        {
            Items = listResult,
            PageSize = pageSize,
            PageIndex = pageIndex,
            TotalItems = totalItems
        };
        return result;
    }

    public async Task<SellerResponse.GetSellerByIdResponse?> GetSellerById(Guid id)
    {
        var query = _dbContext.Sellers
            .Where(x => x.User.Id == id);
        var selectQuery = query.Select(x => new SellerResponse.GetSellerByIdResponse()
        {
            Email = x.User.Email,
            FirstName = x.User.FirstName,
            LastName = x.User.LastName,
            ImageUrl = x.User.ImageUrl,
            CompanyName = x.CompanyName,
            CompanyAddress = x.CompanyAddress,
            TaxCode = x.TaxCode,
            Address = x.User.Address
        });
        var result = await selectQuery.FirstOrDefaultAsync();
        return result;
    }

    public async Task<string> CreateSeller(SellerRequest.CreateSellerRequest request)
    {
        var existingUserQery = _dbContext.Users.Where(x => x.Email == request.Email);
        bool isExistUser = await existingUserQery.AnyAsync();
        if (isExistUser)
        {
            throw new Exception(Message.UserExistWithMail);
        }

        var user = new Repository.Entity.User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashedPassword = request.Password,
            Role = "Seller"
        };
        _dbContext.Users.Add(user);
        var result =
            await _dbContext.SaveChangesAsync();
        if (result > 0)
        {
            var seller = new Repository.Entity.Seller
            {
                CompanyAddress = request.CompanyAddress,
                CompanyName = request.CompanyName,
                TaxCode = request.TaxCode,
                UserId = user.Id
            };
            _dbContext.Sellers.Add(seller);
            var sellerResult = await _dbContext.SaveChangesAsync();
            await _mailService.SendMail(new MailContent()
            {
                To = request.Email,
                Subject = "Welcome to Tet",
                Body = $"Dear {request.FirstName} {request.LastName},\n\n" +
                       "Thank you for registering as a seller on Tet."
            });
            if (sellerResult > 0) return "Add Seller Success";

            return Message.FailToAddSeller;
        }

        return Message.FailToAddSeller;
    }
}