using FilmsUdemy.DTOs;
using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Actors;

public interface IRepositoryActors
{
    Task<List<Actor>> GetAllActors(PaginationDto paginationDto);
    Task<Actor?> GetActorById(int id);
    Task<List<Actor>> GetActorsByName(string name);
    Task<int> CreateActor(Actor actor);
    Task UpdateActor(Actor actor);
    Task DeleteActor(int id);
    Task<bool> ExistActor(int id);
    Task<List<int>> ExistActors(List<int> ids);
}