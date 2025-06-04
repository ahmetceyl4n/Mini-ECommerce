using eTicaretAPI.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();      // IoC Container'a ne eklenirse çalýþacak çünkü bu komutla çaðrýlýyor
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.WithOrigins("https://localhost:4200", "http://localhost:4200")       //  Allow specific origins
                          .AllowAnyMethod()             // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader());           // Allow any header in the request
});
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

app.UseCors(); // CORS politikasýný uygulamak için

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
