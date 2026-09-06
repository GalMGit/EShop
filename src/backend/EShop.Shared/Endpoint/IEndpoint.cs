using Microsoft.AspNetCore.Routing;

namespace EShop.Shared.Endpoint;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}