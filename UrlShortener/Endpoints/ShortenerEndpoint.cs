using Microsoft.EntityFrameworkCore;
using UrlShortener.Models.Requests;
using UrlShortener.Services.Interfaces;

namespace UrlShortener.Endpoints;

public static class ShortenerEndpoint
{
    public static void MapUrlShortenerEndpoints(this WebApplication app)
    {
        app.MapPost("api/shorten",async (ShortenUrlRequest request, IUrlShorteningService urlShorteningService) =>
            {
                if (!Uri.TryCreate(request.Url, UriKind.RelativeOrAbsolute, out var uri))
                {
                    return Results.BadRequest("The entered URL is invalid");
                }

                var shortenedUrl = await urlShorteningService.GenerateUniqueCode(request.Url);
                return Results.Ok(shortenedUrl);
            }
            );
        
        app.MapGet("api/{code}", async (string code, ApplicationDbContext dbContext) =>
        {
            var shortenedUrl = await dbContext.ShortenedUrls
                .FirstOrDefaultAsync(s => s.Code == code);
            if (shortenedUrl is null)
            {
                return Results.NotFound();
            }
            return Results.Redirect(shortenedUrl.InputUrl);
        });
    }
}