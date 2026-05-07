using Microsoft.AspNetCore.Http;

namespace Tet.Service.MediaService;

public interface IService
{
    public Task<string> UploadImageAsync(IFormFile file);
}