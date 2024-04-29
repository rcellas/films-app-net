namespace FilmsUdemy.DTOs.Films;

public class CreateFilmsDto
{
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public IFormFile? Poster { get; set; }
}