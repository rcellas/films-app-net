using FilmsUdemy.Data;
using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.Errors;

public class RepositoriesErrors : IRepositoriesErrors
{
    private readonly ApplicationDbContext _context;
    public RepositoriesErrors(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateError(Error error)
    {
        _context.Add(error);
        await _context.SaveChangesAsync();
    }
}