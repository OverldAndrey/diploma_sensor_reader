using System;

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
            var usbController = new SensorUSBController("COM4");
            var processorController = new DataProcessorController(4, 2, 2, 2);

            usbController.sensorDataReceived += processorController.sensorDataHandler;
 
            Console.Read();
        }
    }
}