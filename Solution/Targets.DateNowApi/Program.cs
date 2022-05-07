using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/datenow", () =>
{
    return DateTime.UtcNow.ToLongTimeString();
});

app.MapGet("/daysAgo", (StringArray myParameter) =>
{
    if (myParameter.Array.Any(x => string.IsNullOrEmpty(x)))
        return Results.BadRequest();

    var dateNow = DateTime.UtcNow;
    var results = myParameter.Array.Select(x =>
    {
        var canParse = DateTime.TryParse(x, out var dateParsed);
        return (canParse, dateParsed);
    })
    .Where(x => x.canParse)
    .Select(x => (dateNow - x.dateParsed).Days.ToString());
    return Results.Ok(results);
});

app.Run();



public class StringArray
{
    public string[] Array { get; private set; }

    public static ValueTask<StringArray> BindAsync(HttpContext context,
                                                   ParameterInfo parameter)
    {
        var parameterKey = parameter.Name;

        var stringValues = context.Request.Query[parameterKey!].ToArray();
        return ValueTask.FromResult(new StringArray { Array = stringValues });
    }
}