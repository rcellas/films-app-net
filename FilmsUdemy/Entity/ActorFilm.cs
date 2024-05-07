namespace FilmsUdemy.Entity;

public class ActorFilm
{
    public int ActorId { get; set; }
    public Actor Actor { get; set; }
    public int FilmId { get; set; }
    public Film Film { get; set; }
    // Order es el orden en el que aparece el actor en la película y personalizado
    public int Order { get; set; }
    public string Character { get; set; } = null!;
}