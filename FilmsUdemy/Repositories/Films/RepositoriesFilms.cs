using FilmsUdemy.Data;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Utils;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories.Films;

public class RepositoriesFilms : IRepositoryFilms
{
    private readonly ApplicationDBContext _context;
    private readonly HttpContext _httpContext;

    // el httpcontextaccessor nos permite acceder a la información de la petición
    public RepositoriesFilms(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
    {
        this._context = context;
        _httpContext = httpContextAccessor.HttpContext!;
    }
    
    public async Task<List<Film>> GetAllFilms(PaginationDto pagination)
    {
        var queryable = _context.Films.AsQueryable();
        await _httpContext.InsertParametersPaginationInHeader(queryable);
        return await _context.Films.Include(p=>p.Comments).ToListAsync();
    }
    
    public async Task<Film?> GetFilmById(int id)
    {
        // Include nos permite traer la relación de la entidad que estamos consultando, en este caso Comments
        return await _context.Films.Include(p=>p.Comments).AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
    }
    
    public async Task<List<Film>> GetFilmsByName(string name)
    {
        return await _context.Films.Where(x=>x.Title.Contains(name)).ToListAsync();
    }
    
    public async Task<int> CreateFilm(Film film)
    {
        _context.Add(film);
        await _context.SaveChangesAsync();
        return film.Id;
    }
    
    public async Task<bool> ExistFilm(int id)
    {
        return await _context.Films.AnyAsync(x=>x.Id == id);
    }
    
    public async Task UpdateFilm(Film film)
    {
        _context.Update(film);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteFilm(int id)
    {
        await _context.Films.Where(x=>x.Id == id).ExecuteDeleteAsync();
    }
}