using FluentValidation;

namespace Deposita.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .PrecisionScale(18, 2, false);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.TransactionType)
            .IsInEnum();
    }
}