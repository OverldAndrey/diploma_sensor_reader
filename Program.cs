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
            var usbController = new SensorUSBController("COM6");
            var processorController = new DataProcessorController();
            
            processorController.readings = Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), 100);

            usbController.sensorDataReceived += processorController.sensorDataHandler;
 
            Console.Read();
        }
    }
}