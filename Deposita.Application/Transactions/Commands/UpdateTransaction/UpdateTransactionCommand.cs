using Deposita.Application.Common.Exceptions;
using Deposita.Application.Common.Interfaces;
using Deposita.Domain.Entities;
using Deposita.Domain.Enums;
using MediatR;

namespace Deposita.Application.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    int Id,
    decimal Amount,
    string Description,
    TransactionType TransactionType) : IRequest;

public class UpdateTransactionCommandHandler
    : IRequestHandler<UpdateTransactionCommand>
{
    private readonly ITransactionRepositoryAsync _repository;

    public UpdateTransactionCommandHandler(ITransactionRepositoryAsync repository)
        => _repository = repository;

    public async Task Handle(
        UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(Transaction), request.Id);

        transaction.Update(request.Amount, request.Description, request.TransactionType);

        await _repository.UpdateAsync(transaction, cancellationToken);
    }
}