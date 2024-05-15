namespace FilmsUdemy.Entity;

public class Error
{
    public Guid Id { get; set; }
    public string Message { get; set; } = null!;
    public string? StackTrace { get; set; } = null!;
    public DateTime Date { get; set; }
}