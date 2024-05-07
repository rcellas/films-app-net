using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories;

public interface IRespostoryGenderFilm
{
    // con este método, devolverá una lista de GenderFilms
    Task<List<Gender>> GetAll();
    
    // con este método, devolverá un GenderFilms
    Task<Gender?> GetById(int id);
    
    //Task lo que hace es que se ejecuta de manera asincrona y no bloquea el hilo principal de la aplicación
    Task<int> Create(Gender gender);
    // Lo que nos hará esto es mirar si existe ese id
    Task<bool> Exist(int id);

    Task Update(Gender gender);
    
    Task Delete(int id);
    
    Task<List<int>> ExistsListGenders(List<int> ids);

}