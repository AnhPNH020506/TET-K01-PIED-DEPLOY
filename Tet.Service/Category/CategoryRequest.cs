namespace Tet.Service.Category;

public class CategoryRequest
{
    public class CreateCategoryRequest
    {
        public required Guid ParentId { get; set; }
        public required string Name { get; set; }
        
    }

    public class UpdateCategoryRequest : CreateCategoryRequest
    {
        public Guid Id  { get; set; }
    }
}