using System;
using System.Collections.Generic;
using System.Linq;

namespace TestSensors
{
    class Program
    {
        private IEnumerable<IEnumerable<short>> readings;
        private DatasetWriterController dataSetWriter = new DatasetWriterController(AppDomain.CurrentDomain.BaseDirectory
             + "/data.txt");
        private SignalClassifierController classifier = new SignalClassifierController();
        
        static void Main(string[] args)
        {
            var prog = new Program();
        }

        private Program()
        {
            var usbController = new SensorUSBController("COM5");
            
            readings = Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), 100);

            usbController.sensorDataReceived += sensorDataHandler;
 
            Console.Read();
        }

        private void sensorDataHandler(object sender, SensorUSBEventArgs eventArgs)
        {
            var data = eventArgs.sensorData.Split(' ').Select(d => Convert.ToInt16(d)).ToArray();
            
            readings = readings.Append(data).Skip(1).ToArray();
            
            if (readings.ToArray()[readings.ToArray().Length - 1].All(d => d == 0)
                && readings.ToArray()[readings.ToArray().Length - 2].Any(d => d != 0))
            {
                dataSetWriter.writeData(readings);
            
                var frame = readings.Skip(
                    Array.FindLastIndex(readings.SkipLast(1).ToArray(),
                        d => d.All(dd => dd == 0)));

                var adjustedFrame =
                    Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), 50).ToArray();

                var frameArray = frame.ToArray();
                for (int i = 0; i < (frameArray.Length > 50 ? adjustedFrame.Length : frameArray.Length); i++)
                {
                    adjustedFrame[i] = frameArray[i];
                }
            
                Console.WriteLine(classifier.predict(classifier.convertFrame(adjustedFrame)));
            }
        }
    }
}