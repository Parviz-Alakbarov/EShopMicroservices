using Catalog.API.Products.UpdateProduct;

namespace Catalog.API.Products.DeleteProduct;
public record DeleteProductCommand(Guid Id)
    : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);
public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required");
    }
}

internal class DeleteProductCommandHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await session.Query<Product>().FirstOrDefaultAsync(x => x.Id == command.Id);
        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        return new(true);
    }
}
