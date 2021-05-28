using System;
using Microsoft.Extensions.Configuration;
using RobotSenderSample;

namespace TestSensors
{
    class Program
    {
        static void Main(string[] args)
        {
            var prog = new Program();
        }

        private Program()
        {
            Init();
            
            Console.Read();
        }

        private async void Init()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            
            var configuration = configurationBuilder.Build();
            
            var usbController = new SensorUSBController(configuration.GetValue<string>("ControllerPort"));
            var processorController = new DataProcessorController(4, 2, 2, 2);
            var scenarioConverter = new ScenarioConverterController();
            var robotController = new RobotClientController();
            
            scenarioConverter.scenarioReady += robotController.scenarioHandler;

            processorController.sensorDataProcessed += scenarioConverter.convertLabelToScenario;

            usbController.sensorDataReceived += processorController.sensorDataHandler;
            
            robotController.Init();
        }
    }
}