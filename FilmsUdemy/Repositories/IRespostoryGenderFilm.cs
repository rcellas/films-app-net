using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories;

public interface IRespostoryGenderFilm
{
    // con este método, devolverá una lista de GenderFilms
    Task<List<GenderFilms>> GetAll();
    
    // con este método, devolverá un GenderFilms
    Task<GenderFilms?> GetById(int id);
    
    //Task lo que hace es que se ejecuta de manera asincrona y no bloquea el hilo principal de la aplicación
    Task<int> Create(GenderFilms genderFilms);
    // Lo que nos hará esto es mirar si existe ese id
    Task<bool> Exist(int id);

    Task Update(GenderFilms genderFilms);
    
    Task Delete(int id);
}