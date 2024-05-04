namespace FilmsUdemy.Entity;

public class GendersFilms
{
    public int GenderId { get; set; }
    public int FilmId { get ; set; }
    public Gender Gender { get; set; } = null!;
    public Film Film { get; set; } = null!;
}