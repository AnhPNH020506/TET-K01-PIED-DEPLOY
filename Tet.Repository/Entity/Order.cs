using Tet.Repository.Abstractions;

namespace Tet.Repository.Entity;

public class Order: BaseEntity<Guid>, IAuditableEntity
{
    public decimal TotalAmount { get; set; }
    public string status { get; set; } = "Pending";//Pending, Processingm Completed, Cancelled
    public required string Address { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }//tạo khóa ngoại liên kết với user
    
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}