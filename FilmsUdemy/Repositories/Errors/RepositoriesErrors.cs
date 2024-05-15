using FilmsUdemy.Data;
using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Errors;

public class RepositoriesErrors : IRepositoriesErrors
{
    private readonly ApplicationDBContext _context;
    public RepositoriesErrors(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task CreateError(Error error)
    {
        _context.Add(error);
        await _context.SaveChangesAsync();
    }
}