using Deposita.Application.Common.Exceptions;
using Deposita.Application.Common.Interfaces;
using Deposita.Domain.Entities;
using MediatR;

namespace Deposita.Application.Transactions.Queries.GetTransactionById;

public record GetTransactionByIdQuery(int Id) : IRequest<TransactionDto>;

public class GetTransactionByIdQueryHandler
    : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly ITransactionRepositoryAsync _repository;

    public GetTransactionByIdQueryHandler(ITransactionRepositoryAsync repository)
        => _repository = repository;

    public async Task<TransactionDto> Handle(
        GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(Transaction), request.Id);

        return TransactionDto.FromEntity(transaction);
    }
}