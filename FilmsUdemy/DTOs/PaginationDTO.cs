namespace FilmsUdemy.DTOs;

public class PaginationDto
{
    public int Page { get; set; } = 1;
    private int _recordsForPage = 10;
    private readonly int _maxRecordsForPage = 50;
    
    public int RecordsForPage
    {
        get
        {
            return _recordsForPage;
        }
        set
        {
            _recordsForPage = (value > _maxRecordsForPage) ? _maxRecordsForPage : value;
        }
    }
}