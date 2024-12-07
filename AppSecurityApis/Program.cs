using MyJwtTokenAuthentication.Models;
using MyJwtTokenAuthentication.Registrations;

var builder = WebApplication.CreateBuilder(args);


#region Configure environment-specific settings
var environment = builder.Environment.EnvironmentName;
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional:false, reloadOnChange:true)
    .AddJsonFile($"appsettings.{environment}.json", optional:true, reloadOnChange:true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);
#endregion

//Map different Configuration settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

//Add services from custom libraries
builder.Services.AddMyLibraryServices();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
