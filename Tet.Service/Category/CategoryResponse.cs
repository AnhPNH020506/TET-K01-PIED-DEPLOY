namespace Tet.Service.Category;

public class CategoryResponse
{
    public class GetCategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid? ParentId { get; set; }
    }
}