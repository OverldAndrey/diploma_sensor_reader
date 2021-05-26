using System;

namespace RobotSenderSample.Messages
{
    public class BmlStatusLogMessage
    {
        public DateTime Time { get; set; }

        public EventLogType Event { get; set; }

        public string BmlId { get; set; }

        public string Bml { get; set; }

        public string ScenarioId { get; set; }

        public ProcessStatus Status { get; set; }
    }
}