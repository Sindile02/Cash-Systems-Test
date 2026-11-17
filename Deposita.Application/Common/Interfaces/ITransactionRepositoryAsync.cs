using Deposita.Domain.Entities;

namespace Deposita.Application.Common.Interfaces;

public interface ITransactionRepositoryAsync
{
    Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
    Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken);
}