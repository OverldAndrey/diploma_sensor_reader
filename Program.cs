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
            var usbController = new SensorUSBController("COM4");
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