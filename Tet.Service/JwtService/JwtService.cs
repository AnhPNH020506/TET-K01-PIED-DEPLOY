using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Tet.Service.JwtService;

public class JwtService : IJwtService
{
    private readonly JwtOptions _jwtOptions = new();

    public JwtService(IConfiguration configuration)
    {
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
        //ánh xạ dữ liệu từ AppSettings vào object JwtOptions
    }
    //claims là 1 thành phần trong payload
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        //tạo 1 key để mã hóa token, sd secretKey từ JwtOptions
        var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        //tạo 1 đối tượng siginigCredentials dder xac dinh thuat tó ma hoa và key sd để ky token
        var tokeOptions = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,//cái token này dc kí -tạo ra ai, tổ chức nào
            audience: _jwtOptions.Audience,//cái token này dành cho ai, to chức nào
            claims: claims,//nhunwng thong tin ma ban muon luu tru trong token
                            //thg la thong tin ve ng dung nhu ID, email,vai trò,v.v
                            //nằm trong payload
            expires: DateTime.Now.AddMinutes(_jwtOptions.ExpireMinutes),
            signingCredentials: signingCredentials
            

        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }

    public ClaimsPrincipal ValidateToken(string token)
    {
        throw new NotImplementedException();
    }
}