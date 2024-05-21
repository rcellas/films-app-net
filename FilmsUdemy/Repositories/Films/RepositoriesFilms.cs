using AutoMapper;
using FilmsUdemy.Data;
using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;
using FilmsUdemy.Utils;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories.Films;

public class RepositoriesFilms : IRepositoryFilms
{
    private readonly ApplicationDbContext _context;
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;
   

    // el httpcontextaccessor nos permite acceder a la información de la petición
    public RepositoriesFilms(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper
        )
    {
        this._context = context;
        this._mapper = mapper;
        _httpContext = httpContextAccessor.HttpContext!;
    }
    
    public async Task<List<Film>> GetAllFilms(PaginationDto pagination)
    {
        var queryable = _context.Films.AsQueryable();
        await _httpContext.InsertParametersPaginationInHeader(queryable);
        return await _context.Films.Include(p=>p.Comments).Include(p=>p.GendersFilms).ThenInclude(gf=>gf.Gender).Include(p=>p.ActorFilms).ThenInclude(af=>af.Actor).ToListAsync();
    }
    
    public async Task<Film?> GetFilmById(int id)
    {
        // Include nos permite traer la relación de la entidad que estamos consultando, en este caso Comments
        // ThenInclude nos permite traer la relación de la entidad que estamos consultando, en este caso GendersFilms
        return await _context.Films.Include(p=>p.Comments).Include(p=>p.GendersFilms).ThenInclude(gf=>gf.Gender).Include(p=>p.ActorFilms.OrderBy(a=>a.Order)).ThenInclude(af=>af.Actor).AsNoTracking().FirstOrDefaultAsync(x=>x.Id == id);
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

    public async Task AssignGender(int id, List<int> gendersIds)
    {
        var film = await _context.Films.Include(p=>p.GendersFilms).FirstOrDefaultAsync(p=> p.Id == id);
        if (film is null)
        {
            throw new AggregateException($"No existe la película con el id {id}");
        }
        // lo que estamos es generar una lista de GendersFilms con los id de los géneros que nos llegan
        var gendersFilms = gendersIds.Select(genderId => new GendersFilms() { GenderId = genderId });
        
        // nosotros podemos tener 3 escenarios
        // 1. Que no tengamos géneros asociados a la película
        // 2. Que tengamos géneros asociados a la película y queramos añadir más
        // 3. Que tengamos géneros asociados a la película y queramos quitar alguno
        // con esta linea de codigo mapeamos nuestro metodo pelicula hacia nuestra entidad GendersFilms con el cual podremos editar. Manteniendo la misma referencia
        film.GendersFilms = _mapper.Map(gendersFilms, film.GendersFilms);
        await _context.SaveChangesAsync();
        
    }
    
    public async Task AssignActors(int id, List<ActorFilm> actorFilms)
    {
        // asignamos el orden en el que aparecen los actores en la película
        for(int i = 1; i <= actorFilms.Count; i++)
        {
            actorFilms[i - 1].Order = i;
        }
        var film = await _context.Films.Include(p=>p.ActorFilms).FirstOrDefaultAsync(p=> p.Id == id);
       
        if(film is null)
        {
            throw new AggregateException($"No existe la película con el id {id}");
        }
       
        film.ActorFilms = _mapper.Map(actorFilms, film.ActorFilms);
        await _context.SaveChangesAsync();
    }
}