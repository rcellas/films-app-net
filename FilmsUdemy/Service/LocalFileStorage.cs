namespace FilmsUdemy.Service;

public class LocalFileStorage:IFileStorage
{
    private readonly IWebHostEnvironment env;
    private readonly IHttpContextAccessor httpContextAccessor;
    
    // el IWebHostEnvironment nos permite acceder a la carpeta wwwroot
    // el IHttpContextAccessor nos permite acceder a la url del servidor
    public LocalFileStorage(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        this.env = env;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> Storage(string container, IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        // lo que estamos haciendo es que vamos a guardar la imagen en la carpeta wwwroot/actors
        string folder = Path.Combine(env.WebRootPath, container);
        
        if(!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        string route = Path.Combine(folder, fileName);
        using (var ms= new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            await File.WriteAllBytesAsync(route, fileBytes);
        }
        
        var url= $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
        var fileUrl = Path.Combine(url, container, fileName).Replace("\\", "/");
        return fileUrl;
    }
    
    public  Task Delete(string? route, string container)
    {
        if (string.IsNullOrEmpty(route))
        {
            return Task.CompletedTask;
        }
        
        var fileName = Path.GetFileName(route);
        var fileDirectory = Path.Combine(env.WebRootPath, container, fileName);

        if (File.Exists(fileDirectory))
        {
            File.Delete(fileDirectory);
        }
        
        return Task.CompletedTask;
    }
}