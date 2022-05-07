using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5116");

var callDateNowStep = Step.Create("call datenow", async (context) =>
{
    var response = await httpClient.GetAsync("datenow");
    if (response.IsSuccessStatusCode)
        return Response.Ok(statusCode: (int)response.StatusCode);
    else
        return Response.Fail(statusCode: (int)response.StatusCode);
});

var scenario = ScenarioBuilder.CreateScenario("Call DateNow Api", callDateNowStep)
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(
        LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(1))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();