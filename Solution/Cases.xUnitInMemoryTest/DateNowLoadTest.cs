using Microsoft.AspNetCore.Mvc.Testing;
using NBomber.CSharp;
using Utils;
using Xunit;

namespace Cases.xUnitInMemoryTest
{
    [Collection("Sequential")]
    public class DateNowLoadTest
    {
        internal class DateNowWebApplicationFactory : WebApplicationFactory<Program> { }

        [Fact]
        public void DateLow_RPS_And_MeanMs_ShouldMatchRequirement()
        {
            // arrange
            const int ExpectedRCP = 90;
            const int ExpectedMeanMs = 200;

            using var application = new DateNowWebApplicationFactory();
            using var client = application.CreateClient();
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
    }
}
