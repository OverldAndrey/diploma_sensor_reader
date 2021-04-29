using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace TestSensors
{
    public class SignalClassifierController
    {
        private MLContext mlContext;
        public TransformerChain<MulticlassPredictionTransformer<OneVersusAllModelParameters>> model;
        
        public string[] categories = new string[] {"First", "Second", "Both", "FirstStrong"};
        
        public class SensorFrame
        {
            [LoadColumn(0, 99)]
            [VectorType(100)]
            public Single[] readings { get; set; }

            [LoadColumn(100)]
            public string Label { get; set; }
        }
        
        private class Prediction
        {
            // Original label.
            public uint Label { get; set; }
            // Predicted label from the trainer.
            public uint PredictedLabel { get; set; }
            public float[] Score { get; set; }
        }
        
        public class PredictionResult
        {
            public string prediction;
            public float[] classPercentages;

            public PredictionResult(string _prediction, float[] _classPercentages)
            {
                prediction = _prediction;
                classPercentages = _classPercentages;
            }

            public override string ToString()
            {
                return "Predicted: " + prediction + "\n" +
                       "Scores: " + String.Join(";", classPercentages) + "\n\n";
            }
        }

        public SignalClassifierController()
        {
            mlContext = new MLContext();

            var reader = mlContext.Data.CreateTextLoader<SensorFrame>(separatorChar: ',', hasHeader: false);

            var trainingDataView = reader.Load("data1.txt", "data2.txt", "data12.txt", "data1S.txt");

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.NormalizeMinMax("readings", fixZero: true))
                .Append(mlContext.MulticlassClassification.Trainers
                    .OneVersusAll(mlContext.BinaryClassification.Trainers
                        .LdSvm(featureColumnName: "readings")));

            model = pipeline.Fit(trainingDataView);
            
            Console.WriteLine("Model fitted");
        }

        public PredictionResult predict(short[] data)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(Enumerable.Repeat(
                new SensorFrame()
                    {
                        readings = data.Select(d => (float) d).ToArray()
                    },
                1
                ));

            var transformedData = model.Transform(dataView);

            var predictions =
                mlContext.Data.CreateEnumerable<Prediction>(transformedData, reuseRowObject: false).ToArray();
            
            //Console.WriteLine(predictions[0].PredictedLabel);
            //var test = transformedData.GetColumn<Vector<Single>>(transformedData.Schema.GetColumnOrNull("Score").Value);
            var predictedIndex = predictions[0].PredictedLabel - 1;

            return new PredictionResult(categories[predictedIndex], predictions[0].Score);
            //"Predicted: " + categories[predictedIndex] + "\n" +
            //   "Scores: " + predictions[0].Score[0] + ";" + predictions[0].Score[1] + ";" + 
            //   + predictions[0].Score[2] + ";" + predictions[0].Score[3] + "\n\n";
        }

        public short[] convertFrame(IEnumerable<IEnumerable<short>> frame)
        {
            var newFrame = Enumerable
                .Repeat((short) 0, frame.ToArray().Length * frame.ToArray()[0].ToArray().Length).ToArray();
            var frameArray = frame.ToArray();
            var frameLength = frameArray.Length;

            for (int i = 0; i < frameLength; i++)
            {
                var reading = frameArray[i];
                for (int j = 0; j < reading.ToArray().Length; j++)
                {
                    newFrame[i + j * frameLength] = reading.ToArray()[j];
                }
            }

            return newFrame;
        }
    }
}