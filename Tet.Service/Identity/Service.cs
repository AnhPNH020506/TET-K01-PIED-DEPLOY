using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Tet.Repository;
using Tet.Service.JwtService;

namespace Tet.Service.Identity;

public class Service : IService
{
    private readonly JwtService.IJwtService _jwtService;
    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions = new();

    public Service(IJwtService jwtService, AppDbContext dbContext, IConfiguration configuration)
    {
        _jwtService = jwtService;
        _dbContext = dbContext;
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
    }
    public async Task<Response.IdentityResponse> Login(string email, string password)
    {
        var user = await _dbContext.Users.Include(u => u.Seller)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (user.HashedPassword != password)
        {
            throw new Exception("Invalid password");
        }
        //User nafy chawsc chawn laf toi
        
        //tajo token
        var claims = new List<Claim>()
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("Role", user.Role),
            new Claim(ClaimTypes.Role, user.Role),
            //phai co claim nay de phan quyen cho cac API endpoint, neu thieu claim nay se k phan quyen dc
            new Claim(ClaimTypes.Expired, DateTimeOffset.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes).ToString()),
        };
        if (user.Role == "Seller")
        {
            var seller = await _dbContext.Sellers.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (seller != null)
            {
                claims.Add(new Claim("SellerId", seller.Id.ToString()));
            }
        }
        var token = _jwtService.GenerateAccessToken(claims);
        var result = new Response.IdentityResponse()
        {
            AccessToken = token,
        };
        return result;
    }
}