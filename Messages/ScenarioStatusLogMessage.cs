using System;

namespace RobotSenderSample.Messages
{
    public class ScenarioStatusLogMessage
    {
        public DateTime Time { get; set; }

        public EventLogType Event { get; set; }

        public string ScenarioId { get; set; }

        public string Scenario { get; set; }

        public ProcessStatus Status { get; set; }
    }
}