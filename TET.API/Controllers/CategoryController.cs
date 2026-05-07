using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Template;
using Tet.Repository;
using Tet.Repository.Entity;
using Tet.Service.Category;
using Tet.Service.Models;

//using Tet.Service.User;

namespace TET.API.Controllers;
[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly ICategoryService _categoryService;
    public CategoryController(AppDbContext dbContext, ICategoryService categoryService)
    {
        _dbContext = dbContext;
        _categoryService = categoryService;
    }
    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetCategories()
    {
        var listResult = await _categoryService .GetCategories();
        return Ok(ApiResponseFactory.SuccessResponse(listResult, "Categories retrieved", HttpContext.TraceIdentifier));
    }
    [HttpGet("{parentId}/childrens")]
    public async Task<IActionResult> GetCategoriesById(Guid id)
    {
        var listResult = await _categoryService.GetCategories();
        return Ok(listResult);
    }
    [HttpPost("")]
    public IActionResult CreateCategories([FromBody]CategoryRequest.CreateCategoryRequest
        request)

    {
        var category = new Category()
        {
            ParentId = request.ParentId,
            Name = request.Name
        };
        _dbContext.Categories.Add(category);
        _dbContext.SaveChanges();
        Console.WriteLine(request);
        return Ok("Get all categories");
    }
    [HttpPut("{id}")]
    public IActionResult UpdateCategoriesById(CategoryRequest.UpdateCategoryRequest request)
    {
        Console.WriteLine(request);
        return Ok("Get all categories");
    }
    [HttpDelete("{id}")]
    public IActionResult DeleteCategoriesById(Guid id)
    {
        return Ok("Get all categories");
    }
    
}