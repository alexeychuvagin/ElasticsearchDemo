using ElasticsearchDemo.Api.Models;

namespace ElasticsearchDemo.Api.Interfaces
{
    internal interface INewsService
    {
        Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CancellationToken ct = default);

        Task<IReadOnlyCollection<News>> GetNewsAsync(int page = 0, int pageSize = 0, string? category = default, CancellationToken ct = default);
    }
}