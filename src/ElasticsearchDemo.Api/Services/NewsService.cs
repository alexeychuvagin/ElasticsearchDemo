using System.Text;
using ElasticsearchDemo.Api.Interfaces;
using ElasticsearchDemo.Api.Models;
using Nest;

namespace ElasticsearchDemo.Api.Services
{
    internal sealed class NewsService : INewsService
    {
        private readonly IElasticClient _client;

        public NewsService(IElasticClient client)
        {
            _client = client;
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken ct = default)
        {
            var request = new SearchDescriptor<News>()
                .Index("news")
                .Size(0)
                .Aggregations(aggs => aggs.Terms("categories", term => term.Field(field => field.Category)));

            var searchResult = await _client.SearchAsync<News>(request, ct);

            return searchResult
                .Aggregations.Terms("categories")
                .Buckets.Select(x => new Category(x.Key, x.DocCount ?? 0))
                .ToList();
        }

        public async Task<IReadOnlyCollection<News>> GetNewsAsync(int page = 0, int pageSize = 0, string? category = default, CancellationToken ct = default)
        {
            var request = new SearchDescriptor<News>()
                .Index("news");

            if (!string.IsNullOrWhiteSpace(category))
            {
                request.Query(q => q.Term(t => t.Category, category.ToUpper()));
            }

            if (pageSize > 0)
            {
                var zeroBasedPage = page > 0 ? page - 1 : 0;

                request
                    .Skip(zeroBasedPage * pageSize)
                    .Take(pageSize);
            };

            var json = _client.GetJsonQuery(request);
            var searchResult = await _client.SearchAsync<News>(request, ct);

            return searchResult
                .Hits.Select(x => x.Source)
                .ToList();
        }
    }

    internal static class ElasticClientExtensions
    {
        public static string GetJsonQuery<T>(this IElasticClient client, SearchDescriptor<T> searchDescriptor) where T : class
        {
            using var ms = new MemoryStream();
            client.RequestResponseSerializer.Serialize(searchDescriptor, ms);
            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}