using Inventory.Application.Features.Stock.CreateStock;
using Inventory.Application.Features.Stock.DeleteStock;
using Inventory.Application.Features.Stock.GetAllStock;
using Inventory.Application.Features.Stock.GetStockByProductId;
using Inventory.Application.Features.Stock.UpdateStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public InventoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> Get(Guid productId)
    {
        var result = await _mediator.Send(
            new GetStockByProductIdQuery(productId));

        if (!result.IsSuccess)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllStocksQuery());

        return Ok(result.Value);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> Delete(Guid productId)
    {
        var result = await _mediator.Send(
            new DeleteStockCommand(productId));

        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}