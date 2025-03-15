namespace Catalog.API.Products.DeleteProduct;
public record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
internal class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

        var product = await session.Query<Product>().FirstOrDefaultAsync(x => x.Id == command.Id);
        if (product == null)
        {
            throw new ProductNotFoundException();
        }
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        return new(true);
    }
}
