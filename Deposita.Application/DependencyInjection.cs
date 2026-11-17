using System.Reflection;
using Deposita.Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Deposita.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Registers every AbstractValidator<T> in this project
        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(cfg =>
        {
            // Registers every IRequestHandler in this project
            cfg.RegisterServicesFromAssembly(assembly);

            // Runs validation before every handler
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        return services;
    }
}