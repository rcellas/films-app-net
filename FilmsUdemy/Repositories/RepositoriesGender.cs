using FilmsUdemy.Data;
using FilmsUdemy.Entity;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories;

public class RepositoriesGender : IRespostoryGenderFilm
{
    //esto es un constructor que recibe un parametro de tipo ApplicationDBContext
    // ApplicationDBContext es una clase que nos permite interactuar con la base de datos
    private readonly ApplicationDBContext context;
    
    // con este constructor, le decimos que el context que se recibe por parámetro, se lo asignamos a la variable context
    public RepositoriesGender(ApplicationDBContext context)
    {
        this.context = context;
    }

    public async Task<int> Create(GenderFilms genderFilms)
    {
        // esto no creara el registro en la base de datos, sino que es un paso previo para que se cree
        // nos marcará  como que se ha añadido un registro nuevo
        context.Add(genderFilms);
        // con el await le decimos que espere a que se ejecute el saveChangesAsync
        // y que no bloquee el hilo principal de la aplicación
        // el saveChangesAsync nos guardará el registro en la base de datos de forma asincrona
        // con esta línea de código, se guardará el registro en la base de datos
        await context.SaveChangesAsync();
        // nos devolverá el id del registro que se ha creado
        return genderFilms.Id;
    }
    
    public async Task Update(GenderFilms genderFilms)
    {
        // esto no actualizara el registro en la base de datos, sino que es un paso previo para que se actualice
        // nos marcará  como que se ha modificado un registro
        context.Update(genderFilms);
        // con el await le decimos que espere a que se ejecute el saveChangesAsync
        // y que no bloquee el hilo principal de la aplicación
        // el saveChangesAsync nos guardará el registro en la base de datos de forma asincrona
        // con esta línea de código, se guardará el registro en la base de datos
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        //EL Where nos permite filtrar los registros que coincidan con el id que le pasamos por parámetro
        await context.Genders.Where(x=>x.Id == id).ExecuteDeleteAsync();
    }

    public async Task<bool> Exist(int id)
    {
        // con AnyAsync, lo que estamos diciendo es que nos devuelva un booleano
        return await context.Genders.AnyAsync(x=>x.Id == id);
    }

    

    public async Task<List<GenderFilms>> GetAll()
    {
        return await context.Genders.OrderBy(x=>x.Name).ToListAsync();
    }

    public async Task<GenderFilms?> GetById(int id)
    {
        //lo que estamos diciendo es que nos devuelva el primer registro que coincida con el id que le pasamos por parámetro
        // si no encuentra el registro, devolverá un null
        return await context.Genders.FirstOrDefaultAsync(x=>x.Id == id);
    }
}