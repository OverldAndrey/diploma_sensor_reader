using System;
using Microsoft.Extensions.Configuration;
using RobotSenderSample;

namespace TestSensors
{
    public class RobotClientController
    {
        private RobotHubClient robotCommandClient;
        private IConfiguration configuration;

        private bool scenarioRunningFlag = false;
        public RobotClientController() {}

        public async void Init()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            
            configuration = configurationBuilder.Build();
            
            var commandClient = new RobotHubClient(configuration.GetValue<string>("RobotAddress"));
            robotCommandClient = commandClient;
            await commandClient.StartAsync();

            robotCommandClient.scenarioStatusChanged += scenarioStatusUpdateHandler;
        }

        public async void scenarioHandler(object sender, ScenarioEventArgs eventArgs)
        {
            var scenario = eventArgs.scenario;

            if (!scenarioRunningFlag)
            {
                scenarioRunningFlag = true;
                Console.WriteLine("Adding scenario");
                Console.WriteLine(scenario);
                await robotCommandClient.Add(scenario);
            }
        }

        private void scenarioStatusUpdateHandler(object sender, RobotClientScenarioEventArgs eventArgs)
        {
            Console.WriteLine(eventArgs.scenarioMessage.Status.ToString());
            
            if (eventArgs.scenarioMessage.Status.ToString() == "Removed")
            {
                Console.WriteLine("Set flag to false");
                scenarioRunningFlag = false;
            }

            eventArgs = null;
        }
    }
}