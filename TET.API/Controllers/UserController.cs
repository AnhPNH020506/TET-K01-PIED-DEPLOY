using Microsoft.AspNetCore.Mvc;
using Tet.Repository;
using Tet.Repository.Entity;
using Tet.Service.User;
using MediaService = Tet.Service.MediaService;
namespace TET.API.Controllers;
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IService _userService ;
    private readonly MediaService.IService _mediaService;
    public UserController(AppDbContext dbContext, IService userService, MediaService.IService mediaService)
    {
        _dbContext = dbContext;
        _userService = userService;
        _mediaService = mediaService;
    }
    //HTTP METHOD: GET, POST, PUT,...
    //Param: Query string, Path Param
    
    //Query string: http://localhost:5000User/123
        // name và age là queery string
        //Query string nằm sau dấu ?
    
    //Path (Route) Param: http://localhost:5000User/123
        // 123 là path param hoặc route param
        //Path param nằm trong đường dẫn
    
    //GET là k có body
    //POST, PUT, PATH có body
    
    //Tại sao vì để trasnh lộ thông tin
    //vd: Username, Password
    
    //Chuẩn REST FULL API
    //GET all users 
    //get user by id: GET http://localhost:5000/User/{id}
    //create user: POST http://localhost:5000/User
    //Update User by id: PUT http://localhost:5000/User/{id}
    //Delete User by id: DELETE http://localhost:5000/User/{id}
    [HttpGet("")]
    public async Task<IActionResult> GetUsers([FromQuery] string? searchTerm, int pageSize = 10, int pageIndex = 1)//bỏ vào đây ta được là sau dấu chấm hỏi
    {
        var users = await _userService.GetUsers(searchTerm, pageSize, pageIndex);
        return Ok(users);
    }
    
    
    [HttpGet(template: "{id}")] //"{id}")]-> path param
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }
    
    
    [HttpPost(template: "")]
    public async Task<IActionResult> CreateUser([FromForm] Request.CreateUserRequest request)
    {
        // var user = _dbContext.Users.ToList();
        var user = new User()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            HashedPassword = request.Password, //ch hash chỉ demo
        };
        if(request.Avatar != null)
        {
            var media = await _mediaService.UploadImageAsync(request.Avatar);
            user.ImageUrl = media;
        }
        
        _dbContext.Users.Add(user);
        
        _dbContext.SaveChanges();
        
        Console.WriteLine(request);
        return Ok("Get all users");
    }
    [HttpDelete(template: "{id}")]
    public IActionResult DeleteUserById(Guid id)
    {
        // var user = _dbContext.Users.ToList();
       
        return Ok("Get all users");
    }
    [HttpPut(template: "{id}")]
    public IActionResult UpdateUserById(Guid id, Request.UpdateUserRequest request)
    {
        // var user = _dbContext.Users.ToList();
        Console.WriteLine(request);
        return Ok("Get all users");
    }
}  