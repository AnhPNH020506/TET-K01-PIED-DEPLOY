using Microsoft.EntityFrameworkCore;
using Tet.Repository;

namespace Tet.Service.Category;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;

    public CategoryService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CategoryResponse.GetCategoryResponse>> GetCategories()
    {
        var query = _dbContext.Categories.OrderBy(x => x.Name);
        var selectQuery = query.Select(x => new CategoryResponse.GetCategoryResponse()
        {
            Id =  x.Id,
            Name = x.Name,
            ParentId =  x.ParentId,
        });
        var listResult = await selectQuery.ToListAsync();
        return listResult;
    }

    

    public async Task<List<CategoryResponse.GetCategoryResponse>> GetCategoryById(Guid parentId)
        {
            var query = _dbContext.Categories.Where(x => x.Id == parentId);

            var selectQuery = query.Select(x => new CategoryResponse.GetCategoryResponse()
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                
                // Children = x.Children.Select(y => new CategoryResponse.GetCategoryResponse()
                // {
                //     Id = y.Id,
                //     Name  = y.Name
                // }).ToList()
                
            });
            var listResult = await selectQuery.ToListAsync();
            return listResult;
            
        }
    
}