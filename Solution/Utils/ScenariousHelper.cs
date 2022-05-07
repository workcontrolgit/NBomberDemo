using NBomber.Contracts;
using NBomber.CSharp;

namespace Utils
{
    public static class ScenariousHelper
    {
        public static Scenario GetDateNowTestScenario(HttpClient httpClient)
        {
            var callDateNowStep = Step.Create("call datenow", async (context) =>
            {
                var response = await httpClient.GetAsync("datenow");
                if (response.IsSuccessStatusCode)
                    return Response.Ok(statusCode: (int)response.StatusCode);
                else
                    return Response.Fail(statusCode: (int)response.StatusCode);
            });

            var scenario = ScenarioBuilder.CreateScenario("Call DateNow Api", callDateNowStep)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromSeconds(30))
                );

            return scenario;
        }
    }
}
