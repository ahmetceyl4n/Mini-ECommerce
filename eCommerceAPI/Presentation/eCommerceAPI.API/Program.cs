using eCommerceAPI.Application;
using eCommerceAPI.Application.Validators.Products;
using eCommerceAPI.Infrastructure;
using eCommerceAPI.Infrastructure.Filters;
using eCommerceAPI.Infrastructure.Services.Storage.Azure;
using eCommerceAPI.Infrastructure.Services.Storage.Local;
using eCommerceAPI.Persistence;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        { 
            ValidateAudience = true, // kullanýlacak token deðerlerini hangi origin/sitelerin kullanacaðýný belirlediðimiz deðer
            ValidateIssuer = true, // token'ýn hangi issuer tarafýndan oluþturulduðunu doðrulamak için kullanýlýr. tokeni kim daðýtýyor onu söyler 
            ValidateLifetime = true, // token'ýn geçerlilik süresini doðrulamak için kullanýlýr. tokenýn süresi dolmuþ mu onu kontrol eder
            ValidateIssuerSigningKey = true,  // token'ýn imzasýný doðrulamak için kullanýlýr. tokenýn imzasý doðru mu onu kontrol eder

           ValidAudience = builder.Configuration["Token:Audience"], // Geçerli audience deðerini alýr

           ValidIssuer = builder.Configuration["Token:Issuer"], // Geçerli issuer deðerini alýr

           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])) // Token'ýn imzasýný doðrulamak için kullanýlan anahtar
        };
    });

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

app.UseAuthentication(); // Authentication middleware'ýný kullanmak için
app.UseAuthorization();

app.MapControllers();

app.Run();
