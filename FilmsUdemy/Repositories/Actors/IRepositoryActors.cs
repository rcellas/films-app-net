using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Actors;

public interface IRepositoryActors
{
    Task<List<Actor>> GetAllActors();
    Task<Actor?> GetActorById(int id);
    Task<int> CreateActor(Actor actor);
    Task UpdateActor(Actor actor);
    Task DeleteActor(int id);
    Task<bool> ExistActor(int id);
}