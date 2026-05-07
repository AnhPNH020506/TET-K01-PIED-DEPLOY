using Tet.Repository.Abstractions;

namespace Tet.Repository.Entity;

public class User: BaseEntity<Guid>, IAuditableEntity//Guid trong ngoặc nhọn (genaric)
{//sau dấu : là kế thừa, dấu , là implement
   //Guid: tạo chuỗi ngẫu nhiên
    
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ImageUrl { get; set; } = null;//dấu ? đc quyền để null
    public string? PhoneNumber { get; set; } = null;
    public required string HashedPassword { get; set; }
    public string? Address { get; set; } = null;
    public string Role { get; set; } = "User"; //User, admin, ...
    public bool IsVerify { get; set; } = false;// khi user register, thì phải verify email hợp lệ
    public int VerifyCode { get; set; }// Mã verify gửi về email
    //Soft Delete
    // Tránh xung đột khóa ngoại(Foreign Key)
    
    public Seller? Seller { get; set; }
    public Cart? Cart { get; set; }
    //public Cart Cart2 { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}