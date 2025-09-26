using eCommerceAPI.API.Configurations.ColumnWriters;
using eCommerceAPI.API.Extensions;
using eCommerceAPI.Application;
using eCommerceAPI.Application.Validators.Products;
using eCommerceAPI.Infrastructure;
using eCommerceAPI.Infrastructure.Filters;
using eCommerceAPI.Infrastructure.Services.Storage.Azure;
using eCommerceAPI.Infrastructure.Services.Storage.Local;
using eCommerceAPI.Persistence;
using eCommerceAPI.SignalR;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();      // IoC Container'a ne eklenirse çalýþacak çünkü bu komutla çaðrýlýyor
builder.Services.AddInfrastructureServices();   // Add infrastructure services
builder.Services.AddApplicationServices();
builder.Services.AddSignalRServices();        // SignalR services

// Register the storage service with a specific implementation (LocalStorage,Azure,AWS, etc.)
//builder.Services.AddStorage<LocalStorage>(); // Local storage implementation
builder.Services.AddStorage<AzureStorage>();    // Azure storage implementation

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.WithOrigins("https://localhost:4200", "http://localhost:4200")       //  Allow specific origins
                          .AllowAnyMethod()             // Allow any HTTP method (GET, POST, PUT, DELETE, etc.)
                          .AllowAnyHeader()           // Allow any header in the request
                          .AllowCredentials());        // Allow credentials (cookies, authorization headers, etc.)
});

Logger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/eCommerceAPI-.log", rollingInterval: RollingInterval.Day) // Her gün için ayrý bir log dosyasý oluþturur
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"),"logs",needAutoCreateTable: true, 
    columnOptions:new Dictionary<string, ColumnWriterBase>
    {
        { "message", new RenderedMessageColumnWriter() },
        {"message_template", new MessageTemplateColumnWriter() },
        {"level", new LevelColumnWriter() },
        {"time_stamp", new TimestampColumnWriter() },
        {"exception", new ExceptionColumnWriter() },
        {"log_event", new LogEventSerializedColumnWriter() },
        {"user_name", new UsernameColumnWriter()} 
    })
    .WriteTo.Seq(builder.Configuration["Seq:ServerURL"]) // Seq loglama
    .Enrich.FromLogContext() // LogContext'den gelen bilgileri loglara ekler
    .MinimumLevel.Information() // Minimum log seviyesi
    .CreateLogger();
builder.Host.UseSerilog(logger);

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
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

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),// Token'ýn imzasýný doðrulamak için kullanýlan anahtar

            LifetimeValidator = (before, expires, token, parameters) => expires != null ? expires > DateTime.UtcNow : false ,// Token'ýn süresinin dolup dolmadýðýný kontrol eder. Eðer expires null deðilse ve expires UTC zamanýndan büyükse true döner, aksi halde false döner.

            NameClaimType = ClaimTypes.Name // Token içindeki hangi claim'in kullanýcýnýn adý olarak kabul edileceðini belirtir. Loglamada kullanmak için ekledik. gidip tokenhandlerda claim ekliyoruz

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

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>()); // Global exception handling için

app.UseSerilogRequestLogging(); // HTTP isteklerini loglamak için Serilog middleware'ýný kullanmak için
app.UseHttpLogging(); // HTTP isteklerini loglamak için

app.UseCors("AllowAllOrigins"); // CORS politikasýný uygulamak için

app.UseHttpsRedirection();

app.UseAuthentication(); // Authentication middleware'ýný kullanmak için
app.UseAuthorization();

app.Use(async (context, next) => // Her istek için çalýþacak middleware
{
    var userName = context.User?.Identity?.IsAuthenticated !=  null ||true ? context.User.Identity.Name : null; // Kullanýcý adý bilgisi
    LogContext.PushProperty("user_name", userName); // LogContext'e kullanýcý adý bilgisini ekler
    await next(); // Sonraki middleware'a geçiþ yapar
});

app.MapControllers();
app.MapHubs();

app.Run();
