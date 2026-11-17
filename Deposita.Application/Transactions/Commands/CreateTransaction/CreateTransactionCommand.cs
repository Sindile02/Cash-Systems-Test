using Deposita.Application.Common.Interfaces;
using Deposita.Domain.Entities;
using Deposita.Domain.Enums;
using MediatR;

namespace Deposita.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    decimal Amount,
    string Description,
    TransactionType TransactionType) : IRequest<int>;

public class CreateTransactionCommandHandler
    : IRequestHandler<CreateTransactionCommand, int>
{
    private readonly ITransactionRepositoryAsync _repository;

    public CreateTransactionCommandHandler(ITransactionRepositoryAsync repository)
        => _repository = repository;

    public async Task<int> Handle(
        CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = Transaction.Create(
            request.Amount, request.Description, request.TransactionType);

        await _repository.AddAsync(transaction, cancellationToken);

        return transaction.TransactionId;
    }
}