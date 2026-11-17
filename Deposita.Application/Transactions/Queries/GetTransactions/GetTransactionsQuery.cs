using Deposita.Application.Common.Interfaces;
using MediatR;

namespace Deposita.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery : IRequest<IReadOnlyList<TransactionDto>>;

public class GetTransactionsQueryHandler
    : IRequestHandler<GetTransactionsQuery, IReadOnlyList<TransactionDto>>
{
    private readonly ITransactionRepositoryAsync _repository;

    public GetTransactionsQueryHandler(ITransactionRepositoryAsync repository)
        => _repository = repository;

    public async Task<IReadOnlyList<TransactionDto>> Handle(
        GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetAllAsync(cancellationToken);

        return transactions
            .Select(t => TransactionDto.FromEntity(t))
            .ToList();
    }
}