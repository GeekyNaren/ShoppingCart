using MongoDB.Driver;
using ShoppingCart.Configurations;

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();