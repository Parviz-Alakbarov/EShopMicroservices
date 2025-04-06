namespace Ordering.Application.Orders.Queries.GetOrdersByName;

public class GetOrdersByNameHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .Where(x => x.OrderName.Value != null &&
            x.OrderName.Value.ToLower().Contains(query.Name.ToLower()))
            .OrderBy(x => x.OrderName.Value)
            .ToListAsync(cancellationToken);


        var result = orders.ToOrderDtoList();
        return new GetOrdersByNameResult(result);


    }
}
