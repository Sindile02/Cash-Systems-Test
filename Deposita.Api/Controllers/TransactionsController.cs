using Deposita.Application.Transactions;
using Deposita.Application.Transactions.Commands.CreateTransaction;
using Deposita.Application.Transactions.Commands.DeleteTransaction;
using Deposita.Application.Transactions.Commands.UpdateTransaction;
using Deposita.Application.Transactions.Queries.GetTransactionById;
using Deposita.Application.Transactions.Queries.GetTransactions;
using Deposita.Application.Transactions.Queries.GetTransactionTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Deposita.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TransactionsController : ControllerBase
{
    private readonly ISender _sender;

    public TransactionsController(ISender sender) => _sender = sender;

    /// GET api/transactions
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TransactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var transactions = await _sender.Send(new GetTransactionsQuery(), cancellationToken);
        return Ok(transactions);
    }

    [HttpGet("{id:int}")]

    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var transaction = await _sender.Send(new GetTransactionByIdQuery(id), cancellationToken);
        return Ok(transaction);
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes(CancellationToken cancellationToken)
    {
        var types = await _sender.Send(new GetTransactionTypesQuery(), cancellationToken);
        return Ok(types);
    }

    /// POST api/transactions
    [HttpPost]
    public async Task<IActionResult> Create( [FromBody] CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        var id = await _sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// PUT api/transactions/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command with { Id = id }, cancellationToken);
        return NoContent();
    }

    /// DELETE api/transactions/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _sender.Send(new DeleteTransactionCommand(id), cancellationToken);
        return NoContent();
    }
}