﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TestSensors
{
    public class DataProcessorController
    {
        private static string[] topHeadDatasets =
            {"topHeadDataCaress.txt", "topHeadDataScratch.txt", "topHeadDataPoke.txt",
                "topHeadDataStrike.txt", "topHeadDataOther.txt"};
        private static string[] leftSideHeadDatasets =
            {"leftSideHeadDataCaress.txt", "leftSideHeadDataScratch.txt",
                "leftSideHeadDataPoke.txt", "leftSideHeadDataStrike.txt", "leftSideHeadDataOther.txt"};
        private static string[] rightSideHeadDatasets =
            {"rightSideHeadDataCaress.txt", "rightSideHeadDataScratch.txt", "rightSideHeadDataPoke.txt",
                "rightSideHeadDataStrike.txt", "rightSideHeadDataOther.txt"};
        private static string[] leftHandDatasets =
            {"leftHandDataPoke.txt", "leftHandDataShakeHold.txt", "leftHandDataShakeGrip.txt",
                "leftHandDataOther.txt"};
        private static string[] rightHandDatasets =
            {"rightHandDataPoke.txt", "rightHandDataShakeHold.txt", "rightHandDataShakeGrip.txt",
                "rightHandDataOther.txt"};
        private static string[] frontDatasets =
            {"frontDataCaress.txt", "frontDataScratch.txt", "frontDataPoke.txt", "frontDataOther.txt"};
        
        private static string[] topHeadLabels =
            {"CaressTopHead", "ScratchTopHead", "PokeTopHead", "StrikeTopHead",
                "OtherLowTopHead", "OtherHighTopHead"};
        private static string[] leftSideHeadLabels =
            {"CaressLeftSideHead", "ScratchLeftSideHead", "PokeLeftSideHead", "StrikeLeftSideHead",
                "OtherLowLeftSideHead", "OtherHighLeftSideHead"};
        private static string[] rightSideHeadLabels =
            {"CaressRightSideHead", "ScratchRightSideHead", "PokeRightSideHead", "StrikeRightSideHead",
                "OtherLowRightSideHead", "OtherHighRightSideHead"};
        private static string[] leftHandLabels =
            {"PokeLeftHand", "ShakeHoldLeftHand", "ShakeGripLeftHand", "OtherLowLeftHand", "OtherHighLeftHand"};
        private static string[] rightHandLabels =
            {"PokeRightHand", "ShakeHoldRightHand", "ShakeGripRightHand", "OtherLowRightHand", "OtherHighRightHand"};
        private static string[] frontLabels =
            {"CaressFront", "ScratchFront", "PokeFront", "OtherLowFront", "OtherHighFront"};
        public event EventHandler<DataProcessorEventArgs> sensorDataProcessed;

        private event EventHandler<SensorUSBEventArgs> topHeadSensorDataArrived;
        public IEnumerable<IEnumerable<short>> topHeadReadings;
        private int topHeadSensorQty = 0;
        private DatasetWriterController topHeadDataSetWriter;
        private SignalClassifierController topHeadClassifier =
            new SignalClassifierController("single", "four", topHeadDatasets, topHeadLabels);
        
        private event EventHandler<SensorUSBEventArgs> leftSideHeadSensorDataArrived;
        public IEnumerable<IEnumerable<short>> leftSideHeadReadings;
        private int leftSideHeadSensorQty = 0;
        private DatasetWriterController leftSideHeadDataSetWriter;
        private SignalClassifierController leftSideHeadClassifier =
            new SignalClassifierController("single", "two", leftSideHeadDatasets, leftSideHeadLabels);
        
        private event EventHandler<SensorUSBEventArgs> rightSideHeadSensorDataArrived;
        public IEnumerable<IEnumerable<short>> rightSideHeadReadings;
        private int rightSideHeadSensorQty = 0;
        private DatasetWriterController rightSideHeadDataSetWriter;
        private SignalClassifierController rightSideHeadClassifier =
            new SignalClassifierController("single", "two", rightSideHeadDatasets, rightSideHeadLabels);
        
        private event EventHandler<SensorUSBEventArgs> rightHandSensorDataArrived;
        public IEnumerable<IEnumerable<short>> rightHandReadings;
        private int rightHandSensorQty = 0;
        private DatasetWriterController rightHandDataSetWriter;
        private SignalClassifierController rightHandClassifier =
            new SignalClassifierController("single", "two", rightHandDatasets, rightHandLabels);
        
        private event EventHandler<SensorUSBEventArgs> leftHandSensorDataArrived;
        public IEnumerable<IEnumerable<short>> leftHandReadings;
        private int leftHandSensorQty = 0;
        private DatasetWriterController leftHandDataSetWriter;
        private SignalClassifierController leftHandClassifier =
            new SignalClassifierController("single", "two", leftHandDatasets, leftHandLabels);
        
        private event EventHandler<SensorUSBEventArgs> frontSensorDataArrived;
        public IEnumerable<IEnumerable<short>> frontReadings;
        private int frontSensorQty = 0;
        private DatasetWriterController frontDataSetWriter;
        private SignalClassifierController frontClassifier =
            new SignalClassifierController("single", "two", frontDatasets, frontLabels);

        public DataProcessorController(int _topHeadSensorQty,
            int _handSensorQty, int _frontSensorQty, int _sideHeadSensorQty)
        {
            topHeadReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _topHeadSensorQty), 100);
            topHeadSensorQty = _topHeadSensorQty;
            
            leftSideHeadReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _sideHeadSensorQty), 100);
            leftSideHeadSensorQty = _sideHeadSensorQty;
            
            rightSideHeadReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _sideHeadSensorQty), 100);
            rightSideHeadSensorQty = _sideHeadSensorQty;
            
            rightHandReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _handSensorQty), 100);
            rightHandSensorQty = _handSensorQty;
            
            leftHandReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _handSensorQty), 100);
            leftHandSensorQty = _handSensorQty;
            
            frontReadings = Enumerable.Repeat(Enumerable.Repeat<short>(0, _frontSensorQty), 100);
            frontSensorQty = _frontSensorQty;

            topHeadSensorDataArrived += topHeadSensorDataHandler;
            leftSideHeadSensorDataArrived += leftSideHeadSensorDataHandler;
            rightSideHeadSensorDataArrived += rightSideHeadSensorDataHandler;
            rightHandSensorDataArrived += rightHandSensorDataHandler;
            leftHandSensorDataArrived += leftHandSensorDataHandler;
            frontSensorDataArrived += frontSensorDataHandler;
            
            topHeadDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/topHeadData.txt", topHeadSensorQty);
            leftSideHeadDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/leftSideHeadData.txt", leftSideHeadSensorQty);
            rightSideHeadDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/rightSideHeadData.txt", rightSideHeadSensorQty);
            rightHandDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/rightHandData.txt", rightHandSensorQty);
            leftHandDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/leftHandData.txt", leftHandSensorQty);
            frontDataSetWriter = 
                new DatasetWriterController(
                    AppDomain.CurrentDomain.BaseDirectory + "/frontData.txt", frontSensorQty);
            
            Console.WriteLine("Data processor initialized");
        }

        public void sensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            var data = USBeventArgs.sensorData.Split(' ').Select(d => Convert.ToInt16(d)).ToArray();
            
            // return;

            short[] topHeadData = {data[0], data[1], data[2], data[3]};
            short[] leftSideHeadData = {data[6], data[7]};
            short[] rightSideHeadData = {data[4], data[5]};
            short[] rightHandData = {data[10], data[11]};
            short[] leftHandData = {data[8], data[9]};
            short[] frontData = {data[12], data[13]};
            
            topHeadReadings = topHeadReadings.Append(topHeadData).Skip(1).ToArray();
            leftSideHeadReadings = leftSideHeadReadings.Append(leftSideHeadData).Skip(1).ToArray();
            rightSideHeadReadings = rightSideHeadReadings.Append(rightSideHeadData).Skip(1).ToArray();
            rightHandReadings = rightHandReadings.Append(rightHandData).Skip(1).ToArray();
            leftHandReadings = leftHandReadings.Append(leftHandData).Skip(1).ToArray();
            frontReadings = frontReadings.Append(frontData).Skip(1).ToArray();
            
            // Console.WriteLine(USBeventArgs.sensorData);
            
            // return;

            if (
                topHeadReadings.ToArray()[0].All(d => d == 0)
                && topHeadReadings.ToArray()[1].Any(d => d != 0)
                // readings.ToArray()[readings.ToArray().Length - 1].All(d => d == 0)
                // && readings.ToArray()[readings.ToArray().Length - 2].Any(d => d != 0)
            )
            {
                if (topHeadReadings.All(r => r.All(rr => rr < 40)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // topHeadDataSetWriter.writeData(topHeadReadings);

                    topHeadSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }
            
            if (
                leftSideHeadReadings.ToArray()[0].All(d => d == 0)
                && leftSideHeadReadings.ToArray()[1].Any(d => d != 0)
            )
            {
                if (leftSideHeadReadings.All(r => r.All(rr => rr < 40)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // leftSideHeadDataSetWriter.writeData(leftSideHeadReadings);

                    leftSideHeadSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }
            
            if (
                rightSideHeadReadings.ToArray()[0].All(d => d == 0)
                && rightSideHeadReadings.ToArray()[1].Any(d => d != 0)
            )
            {
                if (rightSideHeadReadings.All(r => r.All(rr => rr < 40)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // rightSideHeadDataSetWriter.writeData(rightSideHeadReadings);

                    rightSideHeadSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }

            if (
                rightHandReadings.ToArray()[0].All(d => d == 0)
                && rightHandReadings.ToArray()[1].Any(d => d != 0)
            )
            {
                if (rightHandReadings.All(r => r.All(rr => rr < 100)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // rightHandDataSetWriter.writeData(rightHandReadings);

                    rightHandSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }

            if (
                leftHandReadings.ToArray()[0].All(d => d == 0)
                && leftHandReadings.ToArray()[1].Any(d => d != 0)
            )
            {
                if (leftHandReadings.All(r => r.All(rr => rr < 100)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // leftHandDataSetWriter.writeData(leftHandReadings);

                    leftHandSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }

            if (
                frontReadings.ToArray()[0].All(d => d == 0)
                && frontReadings.ToArray()[1].Any(d => d != 0)
            )
            {
                if (frontReadings.All(r => r.All(rr => rr < 100)))
                {
                    Console.WriteLine("Drop");
                }
                else
                {
                    // frontDataSetWriter.writeData(frontReadings);

                    frontSensorDataArrived.Invoke(this, USBeventArgs);
                }
            }
        }

        private void topHeadSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(topHeadClassifier, topHeadSensorQty, ref topHeadReadings, "topHead");
        }
        
        private void leftSideHeadSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(leftSideHeadClassifier, leftSideHeadSensorQty, ref leftSideHeadReadings, "leftSideHead");
        }
        
        private void rightSideHeadSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(rightSideHeadClassifier, rightSideHeadSensorQty, ref rightSideHeadReadings, "rightSideHead");
        }
        
        private void rightHandSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(rightHandClassifier, rightHandSensorQty, ref rightHandReadings, "rightHand");
        }
        
        private void leftHandSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(leftHandClassifier, leftHandSensorQty, ref leftHandReadings, "leftHand");
        }
        
        private void frontSensorDataHandler(object sender, SensorUSBEventArgs USBeventArgs)
        {
            sensorDataProcessor(frontClassifier, frontSensorQty, ref frontReadings, "front");
        }

        private void sensorDataProcessor(SignalClassifierController classifier, int sensorQty,
            ref IEnumerable<IEnumerable<short>> readings, string robotBodyPart)
        {
            var frame = readings;

            var processingResult = predictSingleFrame(frame, classifier, sensorQty);

            Console.WriteLine(processingResult);
            var eventArgs = new DataProcessorEventArgs();
            if (processingResult.classPercentages.All(cp => cp < 0.6) 
                && !processingResult.prediction.Contains("Strike"))
            {
                switch (robotBodyPart)
                {
                    case "topHead":
                        eventArgs.predictionData = "OtherLowTopHead";
                        break;
                    case "leftSideHead":
                        eventArgs.predictionData = "OtherLowLeftSideHead";
                        break;
                    case "rightSideHead":
                        eventArgs.predictionData = "OtherLowRightSideHead";
                        break;
                    case "rightHand":
                        eventArgs.predictionData = "OtherLowRightHand";
                        break;
                    case "leftHand":
                        eventArgs.predictionData = "OtherLowLeftHand";
                        break;
                    case "front":
                        eventArgs.predictionData = "OtherLowFront";
                        break;
                    default:
                        eventArgs.predictionData = "OtherLowLeftSideHead";
                        break;
                }
            }
            else
            {
                eventArgs.predictionData = processingResult.prediction;
            }
            
            sensorDataProcessed.Invoke(this, eventArgs);

            eventArgs = null;
                
            readings = Enumerable.Repeat(Enumerable.Repeat<short>(0, sensorQty), 100);
        }
        
        private SignalClassifierController.PredictionResult predictFrame(
            IEnumerable<IEnumerable<short>> frame, int frameSize, SignalClassifierController classifier, int sensorQty)
        {
            var adjustedFrame =
                Enumerable.Repeat(Enumerable.Repeat<short>(0, sensorQty), frameSize).ToArray();

            var frameArray = frame.ToArray();
            for (int i = 0; i < (frameArray.Length > frameSize ? adjustedFrame.Length : frameArray.Length); i++)
            {
                adjustedFrame[i] = frameArray[i];
            }

            string strFrameSize = "";
            if (frameSize == 100)
            {
                strFrameSize = "single";
            } 
            else if (frameSize == 200)
            {
                strFrameSize = "double";
            } 
            else
            {
                strFrameSize = "half";
            }
            
            string sensorType = "";
            if (sensorQty == 4)
            {
                sensorType = "four";
            } 
            else
            {
                sensorType = "two";
            }
            
            return classifier.predict(strFrameSize, sensorType, classifier.convertFrame(adjustedFrame));
        }
        
        private SignalClassifierController.PredictionResult predictSingleFrame(
            IEnumerable<IEnumerable<short>> frame, SignalClassifierController classifier, int sensorQty)
        {
            return predictFrame(frame, 100, classifier, sensorQty);
        }
        
        private SignalClassifierController.PredictionResult predictHalfFrame(
            IEnumerable<IEnumerable<short>> frame, SignalClassifierController classifier, int sensorQty)
        {
            return predictFrame(frame, 50, classifier, sensorQty);
        }
        
        private SignalClassifierController.PredictionResult predictDoubleFrame(
            IEnumerable<IEnumerable<short>> frame, SignalClassifierController classifier, int sensorQty)
        {
            return predictFrame(frame, 200, classifier, sensorQty);
        }
    }
    
    public class DataProcessorEventArgs : EventArgs
    {
        public string predictionData { get; set; }
    }
}