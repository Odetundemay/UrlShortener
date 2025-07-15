namespace UrlShortener.Services.Interfaces;

public interface IUrlShorteningService
{
    Task<string> GenerateUniqueCode(string url);
}