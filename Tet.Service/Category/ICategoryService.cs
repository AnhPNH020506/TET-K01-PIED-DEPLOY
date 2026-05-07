namespace Tet.Service.Category;

public interface ICategoryService
{
    public Task<List<CategoryResponse.GetCategoryResponse>> GetCategories();
    public Task<List<CategoryResponse.GetCategoryResponse>> GetCategoryById(Guid parentId);
}