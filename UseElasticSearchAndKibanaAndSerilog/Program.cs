using MyUseElasticSearchAndKibanaAndSerilog.Extensions;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddServices(builder);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

configureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void configureLogging()
{
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    var configuration = new ConfigurationBuilder()
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env}.json", optional: true)
                      .Build();

    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureElasticsSink(configuration, env))
        .Enrich.WithProperty("Environment",env)
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

ElasticsearchSinkOptions ConfigureElasticsSink(IConfigurationRoot configurationRoot,string env)
{
    return new ElasticsearchSinkOptions(new Uri(configurationRoot["ElasticConfiguration:Uri"]))
    {
        AutoRegisterTemplate=true,
        IndexFormat=$"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".","-")}-{env.ToLower()}-{DateTime.UtcNow:yyy-MM}",
        NumberOfReplicas=1,
        NumberOfShards=2
    };
}
