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
            new KeyValuePair<string, string>("CaressTopHead", "topHeadOther.txt"),
            new KeyValuePair<string, string>("ScratchTopHead", "topHeadOther.txt"),
            new KeyValuePair<string, string>("PokeTopHead", "topHeadOther.txt"),
            new KeyValuePair<string, string>("StrikeTopHead", "topHeadOther.txt"),
            new KeyValuePair<string, string>("OtherLowTopHead", "topHeadOther.txt"),
            new KeyValuePair<string, string>("OtherHighTopHead", "topHeadOther.txt"),
        };
        
        private KeyValuePair<string, string>[] leftSideHeadScenarioPairs = {
            new KeyValuePair<string, string>("CaressLeftSideHead", "leftSideHeadOther.txt"),
            new KeyValuePair<string, string>("ScratchLeftSideHead", "leftSideHeadOther.txt"),
            new KeyValuePair<string, string>("PokeLeftSideHead", "leftSideHeadOther.txt"),
            new KeyValuePair<string, string>("StrikeLeftSideHead", "leftSideHeadOther.txt"),
            new KeyValuePair<string, string>("OtherLowLeftSideHead", "leftSideHeadOther.txt"),
            new KeyValuePair<string, string>("OtherHighLeftSideHead", "leftSideHeadOther.txt"),
        };
        
        private KeyValuePair<string, string>[] rightSideHeadScenarioPairs = {
            new KeyValuePair<string, string>("CaressRightSideHead", "rightSideHeadOther.txt"),
            new KeyValuePair<string, string>("ScratchRightSideHead", "rightSideHeadOther.txt"),
            new KeyValuePair<string, string>("PokeRightSideHead", "rightSideHeadOther.txt"),
            new KeyValuePair<string, string>("StrikeRightSideHead", "rightSideHeadOther.txt"),
            new KeyValuePair<string, string>("OtherLowRightSideHead", "rightSideHeadOther.txt"),
            new KeyValuePair<string, string>("OtherHighRightSideHead", "rightSideHeadOther.txt"),
        };
        
        private KeyValuePair<string, string>[] rightHandScenarioPairs = {
            new KeyValuePair<string, string>("PokeRightHand", "rightHandOther.txt"),
            new KeyValuePair<string, string>("ShakeHoldRightHand", "rightHandOther.txt"),
            new KeyValuePair<string, string>("ShakeGripRightHand", "rightHandOther.txt"),
            new KeyValuePair<string, string>("OtherLowRightHand", "rightHandOther.txt"),
            new KeyValuePair<string, string>("OtherHighRightHand", "rightHandOther.txt"),
        };
        
        private KeyValuePair<string, string>[] leftHandScenarioPairs = {
            new KeyValuePair<string, string>("PokeLeftHand", "leftHandOther.txt"),
            new KeyValuePair<string, string>("ShakeHoldLeftHand", "leftHandOther.txt"),
            new KeyValuePair<string, string>("ShakeGripLeftHand", "leftHandOther.txt"),
            new KeyValuePair<string, string>("OtherLowLeftHand", "leftHandOther.txt"),
            new KeyValuePair<string, string>("OtherHighLeftHand", "leftHandOther.txt"),
        };
        
        private KeyValuePair<string, string>[] frontScenarioPairs = {
            new KeyValuePair<string, string>("CaressFront", "frontOther.txt"),
            new KeyValuePair<string, string>("ScratchFront", "frontOther.txt"),
            new KeyValuePair<string, string>("PokeFront", "frontOther.txt"),
            new KeyValuePair<string, string>("OtherLowFront", "frontOther.txt"),
            new KeyValuePair<string, string>("OtherHighFront", "frontOther.txt"),
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