namespace BooksEFCore;

public record Book
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public double Price { get; set; }
}