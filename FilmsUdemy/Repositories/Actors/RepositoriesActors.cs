using FilmsUdemy.Data;
using FilmsUdemy.Entity;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories.Actors;

public class RepositoriesActors : IRepositoryActors
{
    private readonly ApplicationDBContext context;
    
    public RepositoriesActors(ApplicationDBContext context)
    {
        this.context = context;
    }
    
    public async Task<List<Actor>> GetAllActors()
    {
        return await context.Actors.ToListAsync();
    }
    
    public async Task<Actor?> GetActorById(int id)
    {
        //FirstOrDefaultAsync nos devolverá el primer registro que coincida con el id que le pasamos por parámetro
        // el asnotracking nos permite hacer una consulta sin que se quede en memoria
        return await context.Actors.AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
    }
    
    public async Task<int> CreateActor(Actor actor)
    {
        context.Add(actor);
        await context.SaveChangesAsync();
        return actor.Id;
    }
    
    public async Task<bool> ExistActor(int id)
    {
        return await context.Actors.AnyAsync(x=>x.Id == id);
    }
    
    public async Task UpdateActor(Actor actor)
    {
        context.Update(actor);
        await context.SaveChangesAsync();
    }
    
    public async Task DeleteActor(int id)
    {
        await context.Actors.Where(x=>x.Id == id).ExecuteDeleteAsync();
    }
}