using ElasticsearchDemo.Api.Interfaces;

namespace ElasticsearchDemo.Api.Endpoints
{
    internal static class NewsEndpoints
    {
        public static IEndpointRouteBuilder MapNewsEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("/news", GetNews);
            builder.MapGet("/news/{category}", GetNewsByCategory);

            return builder;
        }

        private static async Task<IResult> GetNews(INewsService service, CancellationToken ct, int page = 0, int pageSize = 0)
        {
            var result = await service.GetNewsAsync(page, pageSize, ct: ct);

            return Results.Ok(result);
        }

        private static async Task<IResult> GetNewsByCategory(INewsService service, CancellationToken ct, string category, int page = 0, int pageSize = 0)
        {
            var result = await service.GetNewsAsync(page, pageSize, category, ct: ct);

            return Results.Ok(result);
        }
    }
}