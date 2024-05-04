namespace FilmsUdemy.DTOs.Comment;

public class CommentsDto
{
    public int Id { get; set; }
    public string Body { get; set; } = null!;
    public int FilmId { get; set; }
}