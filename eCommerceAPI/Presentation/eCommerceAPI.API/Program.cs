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
builder.Services.AddPersistenceServices();      // IoC Container'a ne eklenirse �al��acak ��nk� bu komutla �a�r�l�yor
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
    .WriteTo.File("logs/eCommerceAPI-.log", rollingInterval: RollingInterval.Day) // Her g�n i�in ayr� bir log dosyas� olu�turur
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
builder.Services.AddFluentValidationAutoValidation(); // FluentValidation'�n otomatik do�rulama �zelli�ini ekler
builder.Services.AddFluentValidationClientsideAdapters(); // FluentValidation i�in istemci taraf� adapt�rlerini ekler
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductValidator>(); // Register validators from the assembly containing CreateProductValidator

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // kullan�lacak token de�erlerini hangi origin/sitelerin kullanaca��n� belirledi�imiz de�er
            ValidateIssuer = true, // token'�n hangi issuer taraf�ndan olu�turuldu�unu do�rulamak i�in kullan�l�r. tokeni kim da��t�yor onu s�yler 
            ValidateLifetime = true, // token'�n ge�erlilik s�resini do�rulamak i�in kullan�l�r. token�n s�resi dolmu� mu onu kontrol eder
            ValidateIssuerSigningKey = true,  // token'�n imzas�n� do�rulamak i�in kullan�l�r. token�n imzas� do�ru mu onu kontrol eder

            ValidAudience = builder.Configuration["Token:Audience"], // Ge�erli audience de�erini al�r

            ValidIssuer = builder.Configuration["Token:Issuer"], // Ge�erli issuer de�erini al�r

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),// Token'�n imzas�n� do�rulamak i�in kullan�lan anahtar

            LifetimeValidator = (before, expires, token, parameters) => expires != null ? expires > DateTime.UtcNow : false ,// Token'�n s�resinin dolup dolmad���n� kontrol eder. E�er expires null de�ilse ve expires UTC zaman�ndan b�y�kse true d�ner, aksi halde false d�ner.

            NameClaimType = ClaimTypes.Name // Token i�indeki hangi claim'in kullan�c�n�n ad� olarak kabul edilece�ini belirtir. Loglamada kullanmak i�in ekledik. gidip tokenhandlerda claim ekliyoruz

        }; 
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Statik dosyalar� kullanmak i�in

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>()); // Global exception handling i�in

app.UseSerilogRequestLogging(); // HTTP isteklerini loglamak i�in Serilog middleware'�n� kullanmak i�in
app.UseHttpLogging(); // HTTP isteklerini loglamak i�in

app.UseCors("AllowAllOrigins"); // CORS politikas�n� uygulamak i�in

app.UseHttpsRedirection();

app.UseAuthentication(); // Authentication middleware'�n� kullanmak i�in
app.UseAuthorization();

app.Use(async (context, next) => // Her istek i�in �al��acak middleware
{
    var userName = context.User?.Identity?.IsAuthenticated !=  null ||true ? context.User.Identity.Name : null; // Kullan�c� ad� bilgisi
    LogContext.PushProperty("user_name", userName); // LogContext'e kullan�c� ad� bilgisini ekler
    await next(); // Sonraki middleware'a ge�i� yapar
});

app.MapControllers();
app.MapHubs();

app.Run();
