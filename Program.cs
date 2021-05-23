using System;
using System.Collections.Generic;
using System.Linq;

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
            var processorController = new DataProcessorController(14);

            usbController.sensorDataReceived += processorController.sensorDataHandler;
 
            Console.Read();
        }
    }
}