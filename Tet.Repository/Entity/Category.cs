using Tet.Repository.Abstractions;

namespace Tet.Repository.Entity;

public class Category: BaseEntity<Guid>, IAuditableEntity
{
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; } 
    public string Name { get; set; }
    public Guid Id { get; set; }
    //nếu mà là null, thif nó là thg cha cao nhất
    //nếu mà có giá trị thì nó là thằng con của ParentId
    
    
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}