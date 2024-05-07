namespace FilmsUdemy.Entity;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime ReleaseDate { get; set; }
    public string? Poster { get; set; }
    
    //relations
    public List<GendersFilms> GendersFilms { get; set; } = new List<GendersFilms>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
    
    public List<ActorFilm> ActorFilms { get; set; } = new List<ActorFilm>();
}