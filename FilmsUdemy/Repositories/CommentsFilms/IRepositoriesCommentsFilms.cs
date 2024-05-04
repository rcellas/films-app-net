using FilmsUdemy.Entity;

namespace FilmsUdemy.Repositories.CommentsFilms;

public interface IRepositoriesCommentsFilms
{
    Task<List<Comment>> GetAllComments(int filmId);
    Task<Comment?> GetCommentById(int id);
    Task<int> CreateComment(Comment comment);
    Task<bool> ExistComment(int id);
    Task UpdateComment(Comment comment);
    Task DeleteComment(int id);
}