using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace TestSensors
{
    public class ScenarioEventArgs : EventArgs
    {
        public string scenario { get; set; }
    }

    public class ScenarioConverterController
    {
        private KeyValuePair<string, string>[] topHeadScenarioPairs = {
            new KeyValuePair<string, string>("CaressTopHead", "topHeadCaressScenario.txt"),
            new KeyValuePair<string, string>("ScratchTopHead", "topHeadScratchScenario.txt"),
            new KeyValuePair<string, string>("PokeTopHead", "topHeadPokeScenario.txt"),
            new KeyValuePair<string, string>("StrikeTopHead", "topHeadStrikeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowTopHead", "topHeadOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighTopHead", "topHeadOtherScenario.txt"),
        };
        
        private KeyValuePair<string, string>[] leftSideHeadScenarioPairs = {
            new KeyValuePair<string, string>("CaressLeftSideHead", "leftSideHeadCaressScenario.txt"),
            new KeyValuePair<string, string>("ScratchLeftSideHead", "leftSideHeadScratchScenario.txt"),
            new KeyValuePair<string, string>("PokeLeftSideHead", "leftSideHeadPokeScenario.txt"),
            new KeyValuePair<string, string>("StrikeLeftSideHead", "leftSideHeadStrikeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowLeftSideHead", "leftSideHeadOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighLeftSideHead", "leftSideHeadOtherScenario.txt"),
        };
        
        private KeyValuePair<string, string>[] rightSideHeadScenarioPairs = {
            new KeyValuePair<string, string>("CaressRightSideHead", "rightSideHeadCaressScenario.txt"),
            new KeyValuePair<string, string>("ScratchRightSideHead", "rightSideHeadScratchScenario.txt"),
            new KeyValuePair<string, string>("PokeRightSideHead", "rightSideHeadPokeScenario.txt"),
            new KeyValuePair<string, string>("StrikeRightSideHead", "rightSideHeadStrikeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowRightSideHead", "rightSideHeadOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighRightSideHead", "rightSideHeadOtherScenario.txt"),
        };
        
        private KeyValuePair<string, string>[] rightHandScenarioPairs = {
            new KeyValuePair<string, string>("PokeRightHand", "rightHandPokeScenario.txt"),
            new KeyValuePair<string, string>("ShakeHoldRightHand", "rightHandShakeScenario.txt"),
            new KeyValuePair<string, string>("ShakeGripRightHand", "rightHandShakeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowRightHand", "rightHandOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighRightHand", "rightHandOtherScenario.txt"),
        };
        
        private KeyValuePair<string, string>[] leftHandScenarioPairs = {
            new KeyValuePair<string, string>("PokeLeftHand", "leftHandPokeScenario.txt"),
            new KeyValuePair<string, string>("ShakeHoldLeftHand", "leftHandShakeScenario.txt"),
            new KeyValuePair<string, string>("ShakeGripLeftHand", "leftHandShakeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowLeftHand", "leftHandOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighLeftHand", "leftHandOtherScenario.txt"),
        };
        
        private KeyValuePair<string, string>[] frontScenarioPairs = {
            new KeyValuePair<string, string>("CaressFront", "frontCaressScenario.txt"),
            new KeyValuePair<string, string>("ScratchFront", "frontScratchScenario.txt"),
            new KeyValuePair<string, string>("PokeFront", "frontPokeScenario.txt"),
            new KeyValuePair<string, string>("OtherLowFront", "frontOtherScenario.txt"),
            new KeyValuePair<string, string>("OtherHighFront", "frontOtherScenario.txt"),
        };

        private Dictionary<string, string> topHeadMapping;
        private Dictionary<string, string> leftSideHeadMapping;
        private Dictionary<string, string> rightSideHeadMapping;
        private Dictionary<string, string> leftHandMapping;
        private Dictionary<string, string> rightHandMapping;
        private Dictionary<string, string> frontMapping;
        
        public event EventHandler<ScenarioEventArgs> scenarioReady;
        
        public ScenarioConverterController()
        {
            topHeadMapping = new Dictionary<string, string>(topHeadScenarioPairs);
            leftSideHeadMapping = new Dictionary<string, string>(leftSideHeadScenarioPairs);
            rightSideHeadMapping = new Dictionary<string, string>(rightSideHeadScenarioPairs);
            leftHandMapping = new Dictionary<string, string>(leftHandScenarioPairs);
            rightHandMapping = new Dictionary<string, string>(rightHandScenarioPairs);
            frontMapping = new Dictionary<string, string>(frontScenarioPairs);
        }
        
        public void convertLabelToScenario(object sender, DataProcessorEventArgs eventArgs)
        {
            var label = eventArgs.predictionData;
            var labelLow = eventArgs.predictionData.ToLower();

            if (labelLow.Contains("tophead"))
            {
                convertTopHeadLabel(label);
            }
            else if (labelLow.Contains("leftsidehead"))
            {
                convertLeftSideHeadLabel(label);
            }
            else if (labelLow.Contains("rightsidehead"))
            {
                convertRightSideHeadLabel(label);
            }
            else if (labelLow.Contains("righthand"))
            {
                convertRightHandLabel(label);
            }
            else if (labelLow.Contains("lefthand"))
            {
                convertLeftHandLabel(label);
            }
            else if (labelLow.Contains("front"))
            {
                convertFrontLabel(label);
            }
        }

        private void convertTopHeadLabel(string label)
        {
            var scenario = File.ReadAllText(topHeadMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
        
        private void convertLeftSideHeadLabel(string label)
        {
            var scenario = File.ReadAllText(leftSideHeadMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
        
        private void convertRightSideHeadLabel(string label)
        {
            var scenario = File.ReadAllText(rightSideHeadMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
        
        private void convertRightHandLabel(string label)
        {
            var scenario = File.ReadAllText(rightHandMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
        
        private void convertLeftHandLabel(string label)
        {
            var scenario = File.ReadAllText(leftHandMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
        
        private void convertFrontLabel(string label)
        {
            var scenario = File.ReadAllText(frontMapping[label]);

            var evArgs = new ScenarioEventArgs();
            evArgs.scenario = scenario;
            
            scenarioReady.Invoke(this, evArgs);

            evArgs = null;
        }
    }
}