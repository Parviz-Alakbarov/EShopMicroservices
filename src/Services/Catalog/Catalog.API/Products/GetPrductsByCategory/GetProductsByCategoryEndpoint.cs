using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetPrductsByCategory;
//public record GetProductsByCategoryRequest();
public record GetProductsByCategoryResponse(IEnumerable<Product> Products);
public class GetProductsByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("products/category/{category}", async (string Category, ISender sender) =>
        {
            var result = await sender.Send(new GetProductsByCategoryQuery(Category));
            var response = result.Adapt<GetProductsByCategoryResponse>();

            return Results.Ok(response);
        })
        .WithName("GetProductsByCategory")
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Products By Category")
        .WithDescription("Get Products By Category");
    }
}

