using ConfigurationAPI.Repositories;
using ConfigurationAPI.Services;
using ConfigurationLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Configuration API", Version = "v1" });
});
// Add services to the container.
builder.Services.AddDbContext<ConfigurationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpClient(); // Add HttpClient
builder.Services.AddHostedService<BackgroundDataFetcher>(); // Add the background service
builder.Services.AddSingleton<BackgroundServiceManager>(); // Add the background service manager
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Configuration API V1");
        //c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
app.UseCors(); // Enable CORS
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
