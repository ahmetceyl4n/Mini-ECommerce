using eCommerceAPI.Application;
using eCommerceAPI.Application.Validators.Products;
using eCommerceAPI.Infrastructure;
using eCommerceAPI.Infrastructure.Filters;
using eCommerceAPI.Infrastructure.Services.Storage.Azure;
using eCommerceAPI.Infrastructure.Services.Storage.Local;
using eCommerceAPI.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();      // IoC Container'a ne eklenirse çalýþacak çünkü bu komutla çaðrýlýyor
builder.Services.AddInfrastructureServices();   // Add infrastructure services
builder.Services.AddApplicationServices();

// Register the storage service with a specific implementation (LocalStorage,Azure,AWS, etc.)
//builder.Services.AddStorage<LocalStorage>(); // Local storage implementation
builder.Services.AddStorage<AzureStorage>();    // Azure storage implementation

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.WithOrigins("https://localhost:4200", "http://localhost:4200")       //  Allow specific origins
                          .AllowAnyMethod()             // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader());           // Allow any header in the request
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddFluentValidationAutoValidation(); // FluentValidation'ýn otomatik doðrulama özelliðini ekler
builder.Services.AddFluentValidationClientsideAdapters(); // FluentValidation için istemci tarafý adaptörlerini ekler
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>(); // Register validators from the assembly containing CreateProductValidator

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Statik dosyalarý kullanmak için

app.UseCors("AllowAllOrigins"); // CORS politikasýný uygulamak için

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
