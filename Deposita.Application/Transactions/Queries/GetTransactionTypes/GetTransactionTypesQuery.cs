using Deposita.Domain.Enums;
using MediatR;

namespace Deposita.Application.Transactions.Queries.GetTransactionTypes;

public record GetTransactionTypesQuery : IRequest<IReadOnlyList<string>>;

public class GetTransactionTypesQueryHandler
    : IRequestHandler<GetTransactionTypesQuery, IReadOnlyList<string>>
{
    public Task<IReadOnlyList<string>> Handle(
        GetTransactionTypesQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<string> types = Enum.GetNames<TransactionType>();
        return Task.FromResult(types);
    }
}