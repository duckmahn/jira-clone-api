namespace managementapp.Service.Interfaces;
public interface IBlobService
{
    Task<string> SaveFile(string containerName, IFormFile file);
    Task DeleteFile(string containerName, string fileName);
    Task<string> GetFile(string containerName, IFormFile file, string path);
}
