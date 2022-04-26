using ElasticsearchDemo.Api.Interfaces;

namespace ElasticsearchDemo.Api.Endpoints
{
    internal static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder MapCategoriesEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/categories", GetAllCategories);

            return builder;
        }

        private static async Task<IResult> GetAllCategories(INewsService service, CancellationToken ct)
        {
            var result = await service.GetCategoriesAsync(ct);

            return Results.Ok(result);
        }
    }
}