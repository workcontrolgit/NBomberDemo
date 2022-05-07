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

        public static Scenario GetDaysAgoTestScenario(HttpClient httpClient)
        {
            var callDateNowStep = Step.Create("call daysAgo", async (context) =>
            {
                var queryString = GetQueryString();
                var response = await httpClient.GetAsync($"daysAgo{queryString}");
                if (response.IsSuccessStatusCode)
                    return Response.Ok(statusCode: (int)response.StatusCode);
                else
                    return Response.Fail(statusCode: (int)response.StatusCode);
            });

            var scenario = ScenarioBuilder.CreateScenario("Call DaysAgo Api", callDateNowStep)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromSeconds(30))
                );

            return scenario;
        }

        private static string GetQueryString()
        {
            var parametersCount = Random.Shared.Next(0, 3);
            if (parametersCount == 0)
                return string.Empty;
            var parametersString = "?";
            for(int i =0; i< parametersCount-1; i++)
            {
                if (i != 0)
                    parametersString += "&";
                parametersString += $"myParameter={DateTime.Now}";
            }

            return parametersString;
        }
    }
}
