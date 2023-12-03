namespace ChatWeb.Application.Contracts.Infrastructure;

public interface IImageService
{
    string SaveImageFromBase64(string base64);
    Task<string> SaveImageFromURL(string url);
    void RemoveImage(string filename);
}
