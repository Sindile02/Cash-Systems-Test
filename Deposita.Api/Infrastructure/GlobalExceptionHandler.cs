using Deposita.Application.Common.Exceptions;
using Deposita.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Deposita.Api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ProblemDetails problem = exception switch
        {
            ValidationException ve => new ValidationProblemDetails(ve.Errors)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Validation failed"
            },
            NotFoundException nf => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Not found",
                Detail = nf.Message
            },
            DomainException de => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business rule violated",
                Detail = de.Message
            },
            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred"
            }
        };

        if (problem.Status == StatusCodes.Status500InternalServerError)
            _logger.LogError(exception, "Unhandled exception");

        httpContext.Response.StatusCode = problem.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problem, problem.GetType(), cancellationToken);

        return true;
    }
}