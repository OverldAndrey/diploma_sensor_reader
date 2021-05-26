using System;

namespace RobotSenderSample.Messages
{
    public class ScenarioActivationLogMessage
    {
        public DateTime Time { get; set; }

        public EventLogType Event { get; set; }

        public string ScenarioId { get; set; }

        public string Scenario { get; set; }

        public double NewActivation { get; set; }
    }
}