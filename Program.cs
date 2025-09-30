using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;

var builder = WebApplication.CreateBuilder(args);

// Register services with different lifetimes
// Uncomment one at a time to test each scope

// builder.Services.AddSingleton<IMyService, MyService>();  // Singleton
// builder.Services.AddScoped<IMyService, MyService>();    // Scoped
builder.Services.AddTransient<IMyService, MyService>();   // Transient

var app = builder.Build();

// Middleware to demonstrate lifecycle in multiple parts of the pipeline
app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("First Middleware"); 
    await next();
});

app.Use(async (context, next) =>
{
    var myService = context.RequestServices.GetRequiredService<IMyService>();
    myService.LogCreation("Second Middleware"); 
    await next();
});

// Final endpoint to demonstrate service lifecycle in the request
app.MapGet("/", (IMyService myService) =>
{
    myService.LogCreation("Root"); 
    return Results.Ok("Check the console for service creation logs.");
});

app.Run();

public interface IMyService
{
    void LogCreation(string message);
}

public class MyService : IMyService
{
    private readonly int _serviceId;

    public MyService()
    {
        _serviceId = new Random().Next(100000, 999999); // Generate a random 6-digit number
    }

    public void LogCreation(string message)
    {
        Console.WriteLine($"{message} - Service ID: {_serviceId}");
    }
}