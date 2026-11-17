using Deposita.Application.Common.Interfaces;
using Deposita.Infrastructure.Persistence;
using Deposita.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Deposita.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BankingDB")));

        services.AddScoped<ITransactionRepositoryAsync, TransactionRepositoryAsync>();

        return services;
    }
}