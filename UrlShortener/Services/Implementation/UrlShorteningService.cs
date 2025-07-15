using Microsoft.EntityFrameworkCore;
using UrlShortener.Entities;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Services;

public class UrlShorteningService : IUrlShorteningService
{
    public const int NumberOfCharsInShortLink = 7;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    private readonly Random _random = new ();
    private readonly ApplicationDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContext;

    public UrlShorteningService(ApplicationDbContext dbContext,  IHttpContextAccessor httpContext)
    {
        _dbContext = dbContext;
        _httpContext = httpContext;
    }
    public async Task<string> GenerateUniqueCode(string url)
    {
        var codeChars = new char[NumberOfCharsInShortLink];
        while (true)
        {
            for (var i = 0; i < NumberOfCharsInShortLink; i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);
                codeChars[i] = Alphabet[randomIndex];
            }
        
            var newUrl = new string(codeChars);

            if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == newUrl))
            {
                var shortenedUrl = new ShortenedUrl
                {
                    Id = Guid.NewGuid(),
                    InputUrl = url,
                    Code = newUrl,
                    ShortUrl = $"{_httpContext.HttpContext!.Request.Scheme}://{_httpContext.HttpContext.Request.Host}/api/{newUrl}"
                };
                _dbContext.ShortenedUrls.Add(shortenedUrl);
                await _dbContext.SaveChangesAsync();
                
                return shortenedUrl.ShortUrl;
            };   
        }
    }
}