using MongoDB.Driver;
using ProductApis.DbContexts;
using ProductApis.Repository;
using ProductApis.Services;

var builder = WebApplication.CreateBuilder(args);


// Configure environment-specific settings
var environment = builder.Environment.EnvironmentName; // Gets the current environment (e.g., Development, Staging, Production)
Console.WriteLine(environment);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Base configuration
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true) // Environment-specific configuration files, dynamically loads configuration based on the current environment.
    .AddEnvironmentVariables() // Load environment variables into the configuration, allowing you to override settings without modifying the JSON files.
    .AddCommandLine(args); // Load command-line arguments (if any) which can be useful for deployment scripts or testing.

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add APIControllers to swagger
builder.Services.AddControllers();

//Set MongoDb Configuration
builder.Services.Configure<MongoDatabaseSettings>(builder.Configuration.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(m => new MongoClient(builder.Configuration.GetValue<string>("MongoDbSettings:ConnectionString")));
builder.Services.AddScoped<IMongoDatabase>(m => m.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration.GetValue<string>("MongoDbSettings:DatabaseName")));
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IMongoDbContext,MongoDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Add APIControllers to swagger
app.MapControllers();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
