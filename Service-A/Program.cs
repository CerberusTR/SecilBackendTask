using Microsoft.OpenApi.Models;
using Service_A.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<FileConfigurationRepository>(sp =>
{
    return new FileConfigurationRepository("configurationsA.json");
});
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Service A API", Version = "v1" });
});
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Service A API V1");
        //c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
}
app.UseCors(); // Enable CORS
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
