var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/datenow", () =>
{
    return DateTime.UtcNow.ToLongTimeString();
});

app.Run();