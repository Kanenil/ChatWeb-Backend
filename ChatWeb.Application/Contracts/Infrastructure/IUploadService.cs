using Microsoft.AspNetCore.Http;

namespace ChatWeb.Application.Contracts.Infrastructure;

public interface IUploadService
{
    string SaveImageFromBase64(string base64);
    Task<string> SaveFileFromIFormFile(IFormFile formFile);
    Task<string> SaveImageFromURL(string url);
    void RemoveImage(string filename);
}
