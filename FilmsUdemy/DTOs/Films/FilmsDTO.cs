using FilmsUdemy.DTOs.Comment;

namespace FilmsUdemy.DTOs.Films;

public class FilmsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? Poster { get; set; }
    
    public List<CommentsDto> Comments { get; set; } = new List<CommentsDto>();
}
