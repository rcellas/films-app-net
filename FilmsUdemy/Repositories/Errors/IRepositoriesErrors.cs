using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Errors;

public interface IRepositoriesErrors
{
    Task CreateError(Error error);
}