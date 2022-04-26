// https://www.humankode.com/asp-net-core/logging-with-elasticsearch-kibana-asp-net-core-and-docker
using System.Reflection;
using ElasticsearchDemo.Api.Endpoints;
using ElasticsearchDemo.Api.Interfaces;
using ElasticsearchDemo.Api.Services;
using Nest;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var elasticUri = new Uri(builder.Configuration["ElasticConfiguration:Uri"]);
// Serilog.Debugging.SelfLog.Enable(Console.WriteLine);

// Register Serilog
builder.Host.UseSerilog((ctx, cfg) =>
{
    cfg.ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(elasticUri)
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name!.ToLower().Replace(".", "-")}-{environment!.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
            TypeName = null,
            // EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog,
        });
});

// Add services to the container.
builder.Services.AddTransient<IElasticClient>(sp => new ElasticClient(elasticUri));
builder.Services.AddTransient<INewsService, NewsService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();
app.MapCategoriesEndpoints();
app.MapNewsEndpoints();
app.Run();