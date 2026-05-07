using Microsoft.AspNetCore.Http;

namespace Tet.Service.User;

public class Request
{
    public class CreateUserRequest
    {
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public IFormFile? Avatar { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
    }
    public class UpdateUserRequest : CreateUserRequest
    {
        public Guid id { get; set; }
       
    }

    // public class CreateCategoryRequest
    // {
    //     public required Guid ParentId { get; set; }
    //     
    // }
    //
    // public class UpdateCategoryRequest : CreateCategoryRequest
    // {
    //     public Guid id { get; set; }
    // }
}