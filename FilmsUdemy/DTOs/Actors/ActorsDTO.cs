namespace FilmsUdemy.DTOs.Actors;

public class ActorsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    // public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    
    // usamos string pq lo que hacemos es guardar la ruta de la imagen
    public string? Photo { get; set; }
}