using Microsoft.AspNetCore.Http;

namespace EShop.Shared.ResultType;

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
                Results.NotFound(new
                {
                    result.Error.Code,
                    result.Error.Message
                }),

            ErrorType.Validation =>
                Results.BadRequest(new
                {
                    result.Error.Code,
                    result.Error.Message
                }),

            ErrorType.Conflict =>
                Results.Conflict(new
                {
                    result.Error.Code,
                    result.Error.Message
                }),

            ErrorType.Unauthorized =>
                Results.Unauthorized(),

            ErrorType.Forbidden =>
                Results.Forbid(),

            _ =>
                Results.BadRequest(new
                {
                    result.Error.Code,
                    result.Error.Message
                })
        };
    }

    public static IResult ToHttpResponse(
        this ResultType.Result result)
    {
        if (result.IsSuccess)
            return Results.NoContent();

        return result.Error!.Type switch
        {
            ErrorType.NotFound =>
                Results.NotFound(new
                {
                    result.Error.Code,
                    result.Error.Message
                }),

            ErrorType.Validation =>
                Results.BadRequest(new
                {
                    result.Error.Code,
                    result.Error.Message,
                    result.Error.Detail
                }),

            ErrorType.Conflict =>
                Results.Conflict(new
                {
                    result.Error.Code,
                    result.Error.Message
                }),

            ErrorType.Unauthorized =>
                Results.Unauthorized(),

            ErrorType.Forbidden =>
                Results.Forbid(),

            _ =>
                Results.BadRequest(new
                {
                    result.Error.Code,
                    result.Error.Message
                })
        };
    }
}