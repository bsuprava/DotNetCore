using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductMinimalApis.Data;
using ProductMinimalApis.Data.Repositories;
using ProductMinimalApis.Models;
using ProductMinimalApis.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options => 
//                            options.UseSqlServer("YourConnectionStringHere"))
//                                   .AddScoped<IProductRepository, ProductRepository>()
//                                   .AddScoped<ICategoryRepository, CategoryRepository>()
//                                   .AddScoped<IOrderRepository, OrderRepository>()
//                                   .BuildServiceProvider();
builder.Services.AddDbContext<AppDbContext>(options =>
                           options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyShopDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ProductServices>();
builder.Services.AddScoped<CategoryServices>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Hello World!");

//1.Fetch All Products
app.MapGet("/products", (ProductServices productServices) =>
     productServices.GetAllProducts());

//2.Fetch specific Product by Id
app.MapGet("/products/{id}", (int id, ProductServices productServices) =>
     productServices.GetProduct(id));

//3. Creates a New Product
app.MapPost("/products", (Products newproduct, ProductServices productServices) =>
{
    return productServices.AddProduct(newproduct)? Results.Created(): Results.BadRequest();
    
});

//4. Update a specific Product
app.MapPut("/products/{id}", (int id, Products product, ProductServices productServices) =>
{
    return productServices.UpdateProductById(id, product) ? Results.Created() : Results.BadRequest();

});

//5.Delete specific Product by Id
app.MapDelete("/products/{id}", (int id, ProductServices productServices) =>
{
    return productServices.DeleteProductById(id) ? Results.Ok() : Results.BadRequest();
});
     

app.MapPost("/categories", (Categoryies newCategory, CategoryServices categoryServices) =>
{
    return categoryServices.AddCategory(newCategory) ? Results.Created() : Results.BadRequest();

});

app.Run();
