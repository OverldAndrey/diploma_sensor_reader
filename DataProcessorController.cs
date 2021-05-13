using System;
using System.Collections.Generic;
using System.Linq;

namespace TestSensors
{
    public class DataProcessorController
    {
        public IEnumerable<IEnumerable<short>> readings;
        
        private DatasetWriterController dataSetWriter = new DatasetWriterController(AppDomain.CurrentDomain.BaseDirectory
            + "/data.txt");
        private SignalClassifierController classifier = new SignalClassifierController();

        public DataProcessorController()
        {
            
        }

        public void sensorDataHandler(object sender, SensorUSBEventArgs eventArgs)
        {
            var data = eventArgs.sensorData.Split(' ').Select(d => Convert.ToInt16(d)).ToArray();
            
            readings = readings.Append(data).Skip(1).ToArray();
            
            if (
                readings.ToArray()[readings.ToArray().Length - 1].All(d => d == 0)
                && readings.ToArray()[readings.ToArray().Length - 2].Any(d => d != 0)
            )
            {
                dataSetWriter.writeData(readings);
            
                var frame = readings.Skip(
                    Array.FindLastIndex(readings.SkipLast(1).ToArray(),
                        d => d.All(dd => dd == 0))).ToList();

                if (frame.All(framePart => framePart.All(num => num < 100)) || frame.Count < 4)
                {
                    Console.WriteLine("Drop");
                    return;
                }

                Console.WriteLine(predictHalfFrame(frame));
            }
        }
        
        private SignalClassifierController.PredictionResult predictFrame(IEnumerable<IEnumerable<short>> frame, int frameSize)
        {
            var adjustedFrame =
                Enumerable.Repeat(Enumerable.Repeat<short>(0, 2), frameSize).ToArray();

            var frameArray = frame.ToArray();
            for (int i = 0; i < (frameArray.Length > frameSize ? adjustedFrame.Length : frameArray.Length); i++)
            {
                adjustedFrame[i] = frameArray[i];
            }
            
            return classifier.predict(classifier.convertFrame(adjustedFrame));
        }
        
        private SignalClassifierController.PredictionResult predictSingleFrame(IEnumerable<IEnumerable<short>> frame)
        {
            return predictFrame(frame, 100);
        }
        
        private SignalClassifierController.PredictionResult predictHalfFrame(IEnumerable<IEnumerable<short>> frame)
        {
            return predictFrame(frame, 50);
        }
        
        private SignalClassifierController.PredictionResult predictDoubleFrame(IEnumerable<IEnumerable<short>> frame)
        {
            return predictFrame(frame, 200);
        }
    }
}