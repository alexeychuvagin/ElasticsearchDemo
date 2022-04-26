using Nest;

namespace ElasticsearchDemo.Api.Models
{
    public record News
    {
        [Text(Name = "category")]
        public string Category { get; init; } = default!;

        [Text(Name = "headline")]
        public string Headline { get; init; } = default!;

        [Text(Name = "authors")]
        public string Authors { get; init; } = default!;

        [Text(Name = "short_description")]
        public string ShortDescription { get; init; } = default!;

        [Date(Name = "date", Format = "yyyy-MM-dd")]
        public string PublishedAt { get; init; } = default!;
    }
}