namespace FilmsUdemy.Service;

public interface IFileStorage
{
    // le llamamos container pq es el nombre de la carpeta donde se va a guardar la imagen y también pq en Azure se le llama así
    Task Delete(string? route,string container);
    Task<string> Storage(string container, IFormFile file);
    async Task<string> Edit(string route, string container, IFormFile file)
    {
        if (!string.IsNullOrEmpty(route))
        {
            await Delete(route, container);
        }
        return await Storage(container, file);
    }
}