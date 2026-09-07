using Microsoft.AspNetCore.Http;

namespace EShop.Shared.Result;

public static class ResultExtensions
{
    public static IResult ToHttpResponse<T>(
        this Result<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return result.Error!.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            ErrorType.Validation =>
                Results.BadRequest(result.Error),

            ErrorType.Conflict =>
                Results.Conflict(result.Error),

            ErrorType.Unauthorized =>
                Results.Unauthorized(),

            ErrorType.Forbidden =>
                Results.Forbid(),

            _ =>
                Results.BadRequest(result.Error)
        };
    }

    public static IResult ToHttpResponse(
        this Result result)
    {
        if (result.IsSuccess)
            return Results.NoContent();

        return result.Error!.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(result.Error),

            ErrorType.Validation =>
                Results.BadRequest(result.Error),

            ErrorType.Conflict =>
                Results.Conflict(result.Error),

            ErrorType.Unauthorized =>
                Results.Unauthorized(),

            ErrorType.Forbidden =>
                Results.Forbid(),

            _ =>
                Results.BadRequest(result.Error)
        };
    }
}