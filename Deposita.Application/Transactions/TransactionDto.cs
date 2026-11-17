using Deposita.Domain.Entities;
using Deposita.Domain.Enums;

namespace Deposita.Application.Transactions;

public record TransactionDto(
    int TransactionId,
    decimal Amount,
    string Description,
    TransactionType TransactionType,
    DateTime DateCreated)
{
    public static TransactionDto FromEntity(Transaction transaction)
        => new(
            transaction.TransactionId,
            transaction.Amount,
            transaction.Description,
            transaction.TransactionType,
            transaction.DateCreated);
}