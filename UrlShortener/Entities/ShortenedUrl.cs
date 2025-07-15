namespace UrlShortener.Entities;

public class ShortenedUrl
{
    public Guid Id { get; set; }
    public string InputUrl  { get; set; } =  string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string Code {get; set; } = string.Empty;
    public DateTime Created { get; set; }
}