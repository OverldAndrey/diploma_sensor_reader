using System;

namespace RobotSenderSample.Messages
{
    public class TagErrorLogMessage
    {
        public DateTime Time { get; set; }

        public EventLogType Event { get; set; }

        public string TagId { get; set; }

        public string Tag { get; set; }

        public string BmlId { get; set; }

        public string ScenarioId { get; set; }

        public TagStatus Status { get; set; }

        public string Exception { get; set; }
    }
}