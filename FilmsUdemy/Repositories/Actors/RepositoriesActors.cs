using FilmsUdemy.Data;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Utils;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories.Actors;

public class RepositoriesActors : IRepositoryActors
{
    private readonly ApplicationDbContext _context;
    private readonly HttpContext _httpContext;

    // el httpcontextaccessor nos permite acceder a la información de la petición
    public RepositoriesActors(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        this._context = context;
        _httpContext = httpContextAccessor.HttpContext!;
    }
    
    public async Task<List<Actor>> GetAllActors(PaginationDto pagination)
    {
        var queryable = _context.Actors.AsQueryable();
        await _httpContext.InsertParametersPaginationInHeader(queryable);
        return await _context.Actors.ToListAsync();
    }
    
    public async Task<Actor?> GetActorById(int id)
    {
        //FirstOrDefaultAsync nos devolverá el primer registro que coincida con el id que le pasamos por parámetro
        // el asnotracking nos permite hacer una consulta sin que se quede en memoria ya que .net core por defecto hace un seguimiento de los objetos
        // se pone asnotracking para evitar el error de que el objeto ya está en seguimiento y no se pueda actualizar
        return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
    }
    
    public async Task<List<Actor>> GetActorsByName(string name)
    {
        return await _context.Actors.Where(x=>x.Name.Contains(name)).ToListAsync();
    }
    
    public async Task<int> CreateActor(Actor actor)
    {
        _context.Add(actor);
        await _context.SaveChangesAsync();
        return actor.Id;
    }
    
    public async Task<bool> ExistActor(int id)
    {
        return await _context.Actors.AnyAsync(x=>x.Id == id);
    }
    
    public async Task<List<int>> ExistActors(List<int> ids)
    {
        return await _context.Actors.Where(x=>ids.Contains(x.Id)).Select(x=>x.Id).ToListAsync();
    }
    
    public async Task UpdateActor(Actor actor)
    {
        _context.Update(actor);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteActor(int id)
    {
        await _context.Actors.Where(x=>x.Id == id).ExecuteDeleteAsync();
    }
}