using Deposita.Application.Common.Exceptions;
using Deposita.Application.Common.Interfaces;
using Deposita.Domain.Entities;
using MediatR;

namespace Deposita.Application.Transactions.Commands.DeleteTransaction;

public record DeleteTransactionCommand(int Id) : IRequest;

public class DeleteTransactionCommandHandler
    : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepositoryAsync _repository;

    public DeleteTransactionCommandHandler(ITransactionRepositoryAsync repository)
        => _repository = repository;

    public async Task Handle(
        DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, cancellationToken)
                          ?? throw new NotFoundException(nameof(Transaction), request.Id);

        await _repository.DeleteAsync(transaction, cancellationToken);
    }
}