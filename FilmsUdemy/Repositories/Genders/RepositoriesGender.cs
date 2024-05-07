using FilmsUdemy.Data;
using FilmsUdemy.Entity;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories;


public class RepositoriesGender : IRespostoryGenderFilm
{
    //esto es un constructor que recibe un parametro de tipo ApplicationDBContext
    // ApplicationDBContext es una clase que nos permite interactuar con la base de datos
    private readonly ApplicationDBContext _context;
    
    // con este constructor, le decimos que el context que se recibe por parámetro, se lo asignamos a la variable context
    public RepositoriesGender(ApplicationDBContext context)
    {
        this._context = context;
    }

    public async Task<int> Create(Gender gender)
    {
        // esto no creara el registro en la base de datos, sino que es un paso previo para que se cree
        // nos marcará  como que se ha añadido un registro nuevo
        _context.Add(gender);
        // con el await le decimos que espere a que se ejecute el saveChangesAsync
        // y que no bloquee el hilo principal de la aplicación
        // el saveChangesAsync nos guardará el registro en la base de datos de forma asincrona
        // con esta línea de código, se guardará el registro en la base de datos
        await _context.SaveChangesAsync();
        // nos devolverá el id del registro que se ha creado
        return gender.Id;
    }
    
    public async Task Update(Gender gender)
    {
        // esto no actualizara el registro en la base de datos, sino que es un paso previo para que se actualice
        // nos marcará  como que se ha modificado un registro
        _context.Update(gender);
        // con el await le decimos que espere a que se ejecute el saveChangesAsync
        // y que no bloquee el hilo principal de la aplicación
        // el saveChangesAsync nos guardará el registro en la base de datos de forma asincrona
        // con esta línea de código, se guardará el registro en la base de datos
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        //EL Where nos permite filtrar los registros que coincidan con el id que le pasamos por parámetro
        await _context.Genders.Where(x=>x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> Exist(int id)
    {
        // con AnyAsync, lo que estamos diciendo es que nos devuelva un booleano
        return await _context.Genders.AnyAsync(x=>x.Id == id);
    }

    

    public async Task<List<Gender>> GetAll()
    {
        return await _context.Genders.OrderBy(x=>x.Name).ToListAsync();
    }

    public async Task<Gender?> GetById(int id)
    {
        //lo que estamos diciendo es que nos devuelva el primer registro que coincida con el id que le pasamos por parámetro
        // si no encuentra el registro, devolverá un null
        return await _context.Genders.FirstOrDefaultAsync(x=>x.Id == id);
    }
    
    
    // lo que haremos será comprobar si existe el género que le pasamos por parámetro
    public async Task<List<int>> ExistsListGenders(List<int> ids)
    {
        return await _context.Genders.Where(g=>ids.Contains(g.Id)).Select(g=>g.Id).ToListAsync();
    }
}