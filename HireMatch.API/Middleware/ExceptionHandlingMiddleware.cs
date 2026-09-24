using FluentValidation;
using HireMatch.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace HireMatch.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        context.Response.ContentType = "application/json";

        if (exception is NotFoundException notFoundException)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.NotFound;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = notFoundException.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        if (exception is ForbiddenException forbiddenException)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.Forbidden;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = forbiddenException.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }
        
        if (exception is ConflictException conflictException)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.Conflict;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = conflictException.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.BadRequest;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                message = "Validation failed.",
                errors = validationException.Errors
                    .Select(error => new
                    {
                        field = error.PropertyName,
                        message = error.ErrorMessage
                    })
                    .ToList()
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));

            return;
        }

        context.Response.StatusCode =
            (int)HttpStatusCode.InternalServerError;

        var unexpectedErrorResponse = new
        {
            statusCode = context.Response.StatusCode,
            message = "An unexpected error occurred."
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(unexpectedErrorResponse));
    }
}