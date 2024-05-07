namespace FilmsUdemy.Entity;

public class Actor
{
    public int Id { get;set; }
    public string Name { get; set; } = null!;
    //TODO: FIX LASTNAME
    //public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    // de momento lo dejamos como string, pero en un futuro lo cambiaremos a un tipo de dato que sea un array de bytes
    public string? Photo { get; set; }
    
    public List<ActorFilm> ActorFilms { get; set; } = new List<ActorFilm>();
}