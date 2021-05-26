using Microsoft.Extensions.Configuration;
using RobotSenderSample;

namespace TestSensors
{
    public class RobotClientController
    {
        private RobotHubClient robotCommandClient;
        public RobotClientController() {}

        public async void Init()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            
            var configuration = configurationBuilder.Build();
            
            await using var commandClient = new RobotHubClient(configuration.GetValue<string>("RobotAddress"));
            robotCommandClient = commandClient;
            await commandClient.StartAsync();
        }

        public async void scenarioHandler(object sender, ScenarioEventArgs eventArgs)
        {
            var scenario = eventArgs.scenario;

            await robotCommandClient.Add(scenario);
        }
    }
}