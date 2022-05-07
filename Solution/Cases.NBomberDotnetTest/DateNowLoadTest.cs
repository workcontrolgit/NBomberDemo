using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using NBomber.CSharp;
using Utils;
using Xunit;

namespace Cases.NBomberDotnetTest
{
    [Collection("Sequential")]
    public class DateNowLoadTest
    {
        [Fact]
        public void DateLow_RPS_And_MeanMs_ShouldMatchRequirement()
        {
            // arrange
            const int ExpectedRCP = 90;
            const int ExpectedMeanMs = 200;

            using var client = CreateClient();
            var nBomberScenario = ScenariousHelper.GetDateNowTestScenario(client);

            // act
            var runStatistics = NBomberRunner
                .RegisterScenarios(nBomberScenario)
                .WithoutReports()
                .Run();

            // assert
            var stepStats = runStatistics.ScenarioStats[0].StepStats[0].Ok;
            Assert.True(stepStats.Request.RPS >= ExpectedRCP, "RPC is below expected");
            Assert.True(stepStats.Latency.MeanMs <= ExpectedMeanMs, "Mean request execution time above expected");
        }

        private HttpClient CreateClient()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = configuration.GetValue<string>("BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentException("Cannot read configuration");

            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            return httpClient;
        }
    }
}
