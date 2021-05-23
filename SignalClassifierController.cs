using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using Microsoft.ML.Data.DataView;

namespace TestSensors
{
    public class SignalClassifierController
    {
        private MLContext mlContext;
        public MulticlassPredictionTransformer<OneVersusAllModelParameters> model;
        public EstimatorChain<MulticlassPredictionTransformer<OneVersusAllModelParameters>> estimatorPipeline;
        public TransformerChain<MulticlassPredictionTransformer<OneVersusAllModelParameters>> transformer;

        public string[] categories = new string[] {"Caress", "Scratch", "OtherLow", "OtherHigh"};
        
        public class SensorFrame
        {
            [LoadColumn(0, 14 * 100 - 1)]
            [VectorType(14 * 100)]
            public Single[] readings { get; set; }

            [LoadColumn(14 * 100)]
            public string Label { get; set; }
        }
        
        public class SensorHalfFrame
        {
            [LoadColumn(0, 14 * 50 - 1)]
            [VectorType(14 * 50)]
            public Single[] readings { get; set; }

            [LoadColumn(14 * 50)]
            public string Label { get; set; }
        }
        
        public class SensorDoubleFrame
        {
            [LoadColumn(0, 14 * 200 - 1)]
            [VectorType(14 * 200)]
            public Single[] readings { get; set; }

            [LoadColumn(14 * 200)]
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

        public SignalClassifierController(string frameSize)
        {
            mlContext = new MLContext();

            var reader = getFrameReader(frameSize);

            var trainingDataView = reader.Load("dataCaress.txt", "dataScratch.txt", "dataOther.txt");

            var split = mlContext.Data.TrainTestSplit(trainingDataView, testFraction: 0.2);

            estimatorPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.NormalizeMinMax("readings", fixZero: true))
                .Append(mlContext.MulticlassClassification.Trainers
                    .OneVersusAll(mlContext.BinaryClassification.Trainers
                        .LdSvm(featureColumnName: "readings")));

            transformer = estimatorPipeline.Fit(split.TrainSet);

            var OVAEstimator = mlContext.MulticlassClassification.Trainers
                .OneVersusAll(mlContext.BinaryClassification.Trainers
                    .LdSvm(featureColumnName: "readings"));

            var transformedTrainingData = transformer.Transform(split.TrainSet);

            model = OVAEstimator.Fit(transformedTrainingData);
            
            Console.WriteLine("Model fitted");

            var transformedTestData = transformer.Transform(split.TestSet);
            
            var testPredictions = model.Transform(transformedTestData);
            
            Console.WriteLine(mlContext.MulticlassClassification.Evaluate(testPredictions).ConfusionMatrix.GetFormattedConfusionTable());
        }

        private TextLoader getHalfFrameReader()
        {
            return mlContext.Data.CreateTextLoader<SensorHalfFrame>(separatorChar: ',', hasHeader: false);
        }
        
        private TextLoader getSingleFrameReader()
        {
            return mlContext.Data.CreateTextLoader<SensorFrame>(separatorChar: ',', hasHeader: false);
        }
        
        private TextLoader getDoubleFrameReader()
        {
            return mlContext.Data.CreateTextLoader<SensorDoubleFrame>(separatorChar: ',', hasHeader: false);
        }

        private TextLoader getFrameReader(string frameSize)
        {
            switch (frameSize)
            {
                case "half": return getHalfFrameReader();
                case "double": return getSingleFrameReader();
                default: return getSingleFrameReader();
            }
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

            var transformedData = transformer.Transform(dataView);

            var predictedData = model.Transform(transformedData);

            var predictions =
                mlContext.Data.CreateEnumerable<Prediction>(predictedData, reuseRowObject: false).ToArray();
            
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