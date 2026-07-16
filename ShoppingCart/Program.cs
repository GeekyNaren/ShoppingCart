using MongoDB.Driver;
using ShoppingCart.Configurations;
using ShoppingCart.Interfaces;
using ShoppingCart.Middlewares;
using ShoppingCart.Repositories;
using ShoppingCart.Services;

var builder = WebApplication.CreateBuilder(args);

// Bind MongoSettings
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("MongoSettings"));

var mongoSettings = builder.Configuration.GetSection("MongoSettings").Get<MongoSettings>();

builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoSettings.ConnectionString));

builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoSettings.DatabaseName);
});

// Register repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserService, UserService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();