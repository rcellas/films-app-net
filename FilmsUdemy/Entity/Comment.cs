namespace FilmsUdemy.Entity;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }= null!;
    public int FilmId { get; set; }
}