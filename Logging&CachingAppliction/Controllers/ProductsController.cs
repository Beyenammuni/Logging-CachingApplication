using Logging_CachingApplication.Common.Interfaces;
using Logging_CachingApplication.Product.Commands.CreateProduct;
using Logging_CachingApplication.Product.Commands.DeleteProduct;
using Logging_CachingApplication.Product.Commands.UpdateProduct;
using Logging_CachingApplication.Product.Queries.GetProductById;
using Logging_CachingApplication.Product.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Logging_CachingApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductsController> _logger;
    private readonly ITelegramService _telegram;

    public ProductsController(
        IMediator mediator,
        ILogger<ProductsController> logger,
        ITelegramService telegram)
    {
        _mediator = mediator;
        _logger = logger;
        _telegram = telegram;
    }
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command)
    {
        _logger.LogInformation(
            "Creating product: {ProductName}",
            command.Name);

        var result = await _mediator.Send(command);

        _logger.LogInformation(
            "Product created successfully");

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation(
            "Fetching all products");

        var products =
            await _mediator.Send(new GetProductsQuery());

        _logger.LogInformation(
            "Successfully retrieved {Count} products",
            products.Count());

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogInformation("Fetching product {ProductId}", id);

        var result =await _mediator.Send(new GetProductByIdQuery(id));

        if (result == null)
        {
            _logger.LogWarning(
                "Product {ProductId} was not found", id);

            return NotFound();
        }

        _logger.LogInformation("Product {ProductId} retrieved successfully", id);

        return Ok(result);
    }

    [HttpGet("telegram-test")]
    public async Task<IActionResult> TelegramTest()
    {
        await _telegram.SendMessageAsync("Hello from ASP.NET Core!", CancellationToken.None);

        _logger.LogInformation("Telegram test message sent");

        return Ok("Telegram message sent.");
    }


    [HttpGet("test-error")]
    public IActionResult TestError()
    {
        throw new Exception(
            "Test error from Global Exception Middleware");
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id)
        {
            _logger.LogWarning("Route Id {RouteId} does not match command Id {CommandId}",
                id, command.Id);

            await _telegram.SendMessageAsync($"Route Id {id} does not match product Id {command.Id}", CancellationToken.None);

            return BadRequest("Route id and payload id do not match.");
        }

        _logger.LogInformation("Updating product {ProductId}", id);

        var result = await _mediator.Send(command);

        _logger.LogInformation("Product {ProductId} updated successfully", id);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation("Deleting product {ProductId}", id);
            

        await _mediator.Send(new DeleteProductCommand(id));

        _logger.LogInformation("Product {ProductId} deleted successfully", id);

        await _telegram.SendMessageAsync("Product {ProductId} deleted successfully", CancellationToken.None);

        return NoContent();
    }
}