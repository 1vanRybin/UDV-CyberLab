using Core.Middlewares;
using ExampleCore.Swagger;
using Infrastucture;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddSwaggerStartUpBase();
builder.Services.TryAddServices();
builder.Services.TryAddInfrastucture(builder.Configuration);
builder.Services.AddCors(cors =>

    cors.AddDefaultPolicy(policy => policy
    .WithOrigins("https://localhost:7023",
    "http://90.156.168.7:8080",
    "https://neolab.aydlioh.ru",
    "http://localhost:5173",
    "http://localhost:4173")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()));


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

app.UseMiddleware<ExceptionHandlerMiddleware>();


app.Run();
