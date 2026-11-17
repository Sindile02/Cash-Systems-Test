using FluentValidation;

namespace Deposita.Application.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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