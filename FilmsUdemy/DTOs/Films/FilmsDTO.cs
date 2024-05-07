using FilmsUdemy.DTOs.Comment;
using FilmsUdemy.Entity;

namespace FilmsUdemy.DTOs.Films;

public class FilmsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? Poster { get; set; }
    
    public List<GenderDto> Gender { get; set; } = new List<GenderDto>();
    public List<CommentsDto> Comments { get; set; } = new List<CommentsDto>();
    public List<ActorFilmDto> Actor { get; set; } = new List<ActorFilmDto>(); 
}
