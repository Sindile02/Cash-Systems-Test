using Deposita.Application.Common.Interfaces;
using Deposita.Domain.Entities;
using Deposita.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Deposita.Infrastructure.Repositories;

public class TransactionRepositoryAsync : ITransactionRepositoryAsync
{
    private readonly BankingDbContext _context;

    public TransactionRepositoryAsync(BankingDbContext context) => _context = context;

    public async Task<IReadOnlyList<Transaction>> GetAllAsync(CancellationToken cancellationToken)
        => await _context.Transactions
            .AsNoTracking()
            .OrderByDescending(t => t.DateCreated)
            .ToListAsync(cancellationToken);

    public async Task<Transaction?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _context.Transactions.FindAsync(new object[] { id }, cancellationToken);

    public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync(cancellationToken);
    }
}