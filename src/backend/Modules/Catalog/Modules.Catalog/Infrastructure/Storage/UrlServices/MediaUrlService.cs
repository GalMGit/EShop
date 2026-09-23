using Modules.Catalog.Application.Abstractions.IServices.IMediaServices;


namespace Modules.Catalog.Infrastructure.Storage.UrlServices;

public sealed class MediaUrlService(
    IPublicStorage publicStorage) 
    : IMediaUrlService
{
    public string? GetUrl(string? relativePath)
        => string.IsNullOrWhiteSpace(relativePath) 
            ? null 
            : publicStorage.GetPublicUrl(relativePath);

    public string? GetThumbnailUrl(string? relativePath)
        => string.IsNullOrWhiteSpace(relativePath) 
            ? null 
            : publicStorage.GetPublicUrl(relativePath);
}