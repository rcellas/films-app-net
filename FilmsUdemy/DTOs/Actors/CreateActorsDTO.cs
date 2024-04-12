namespace FilmsUdemy.DTOs.Actors;

public class CreateActorsDTO
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    
   // el IformFile es un tipo de dato que nos permite recibir archivos desde el cliente
    public IFormFile? Photo { get; set; }
}