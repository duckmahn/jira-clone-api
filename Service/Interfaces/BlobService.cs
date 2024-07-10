

using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace managementapp.Service.Interfaces;
public class BlobService : IBlobService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string _connectionString = "DefaultEndpointsProtocol=https;AccountName=purpleboard;AccountKey=LoUY8l0jEZtW4uPYwIb1IfKJO2wM7YROkfvWlER+ggf1baplxCloXmAOXYxhEipQ4EIRU1kj5I9R+AStSZWrig==;EndpointSuffix=core.windows.net";

    public BlobService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Task DeleteFile(string containerName, string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetFile(string containerName, IFormFile file, string path)
    {
        await DeleteFile(containerName, path);
        return await SaveFile(containerName, file);
    }

    public async Task<string> SaveFile(string containerName, IFormFile file)
    {
        containerName = "photo";
        string connectionString = _connectionString;
        BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
        BlobClient blobClient = container.GetBlobClient(file.FileName);

        var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Position = 0;

        string contentType = GetContentType(file.FileName);

        await blobClient.UploadAsync(ms);
        var path = blobClient.Uri.AbsoluteUri;
        return path;


        //var extension = Path.GetExtension(file.FileName);
        //var fileName = $"{Guid.NewGuid()}{extension}";
        //var folder = Path.Combine("uploads", containerName);

        //if (!Directory.Exists(folder))
        //{
        //    Directory.CreateDirectory(folder);
        //}
        //string fullPath = Path.Combine(folder, fileName);
        //using (var ms = new MemoryStream())
        //{
        //    await file.CopyToAsync(ms);
        //    var fileBytes = ms.ToArray();
        //    await File.WriteAllBytesAsync(fullPath, fileBytes);
        //}

        //var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
        //var filePath = Path.Combine(url, containerName, fileName).Replace("\\", "/");
        //return filePath;
    }
    private string GenerateSasUrl(string containerName, string blobName)
    {
        BlobContainerClient containerClient = new BlobContainerClient(_connectionString, containerName);
        BlobClient blobClient = containerClient.GetBlobClient(blobName);

        // Set the expiration time for the SAS token (e.g., 1 hour)
        DateTimeOffset expiresOn = DateTimeOffset.UtcNow.AddHours(1);

        // Define the SAS token permissions
        BlobSasBuilder sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = expiresOn
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        // Generate the SAS URI
        Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
        return sasUri.ToString();
    }

    private string GetContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            _ => "application/octet-stream",
        };
    }
}
