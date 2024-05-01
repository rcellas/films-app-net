using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Films;

public interface IRepositoryFilms
{
    Task<List<Film>> GetAllFilms(PaginationDto pagination);
    Task<Film?> GetFilmById(int id);
    Task<List<Film>> GetFilmsByName(string name);
    Task<int> CreateFilm(Film film);
    Task UpdateFilm(Film film);
    Task DeleteFilm(int id);
    Task<bool> ExistFilm(int id);
}