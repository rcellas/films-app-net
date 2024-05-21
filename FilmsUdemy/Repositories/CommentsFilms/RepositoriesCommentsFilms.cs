using FilmsUdemy.Data;
using FilmsUdemy.Entity;
using Microsoft.EntityFrameworkCore;

namespace FilmsUdemy.Repositories.CommentsFilms;

public class RepositoriesCommentsFilms : IRepositoriesCommentsFilms
{
    private readonly ApplicationDbContext _context;

    public RepositoriesCommentsFilms(ApplicationDbContext context)
    {
        this._context = context;
    }
    
    public async Task<List<Comment>> GetAllComments(int filmId)
    {
        return await _context.Comments.Where(c => c.FilmId == filmId).ToListAsync();
    }
    
    public async Task<Comment?> GetCommentById(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public async Task<int> CreateComment(Comment comment)
    {
        _context.Add(comment);
        await _context.SaveChangesAsync();
        return comment.Id;
    }
    
    public async Task<bool> ExistComment(int id)
    {
        return await _context.Comments.AnyAsync(x => x.Id == id);
    }
    
    public async Task UpdateComment(Comment comment)
    {
        _context.Update(comment);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteComment(int id)
    {
        await _context.Comments.Where(c=>c.Id == id).ExecuteDeleteAsync();
    }
    
}