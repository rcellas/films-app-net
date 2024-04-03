using System.ComponentModel.DataAnnotations;

namespace FilmsUdemy.Entity;

public class GenderFilms
{
    public int Id{ get; set; }
    // si lo ponemos asi, avisamos de que Name puede ser null sin saltarnos la validación 
    //[StringLength(50)]
    public string Name { get; set; } = null!;
}