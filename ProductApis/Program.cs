using MongoDB.Driver;
using ProductApis.Repository;

var builder = WebApplication.CreateBuilder(args);
//IConfiguration configuration = builder.Configuration.GetConnectionString();
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
