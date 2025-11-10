using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class LoggingManager
    {
        public static void CreateEventLogSource()
        {
            try
            {
                if (!EventLog.SourceExists(AppConfig.EventLogSource))
                {
                    EventLog.CreateEventSource(AppConfig.EventLogSource, "Application");
                }
            }
            catch
            { }
        }

        public static void LogInfo(string message)
        {
            EventLog.WriteEntry(AppConfig.EventLogSource, message, EventLogEntryType.Information);
        }

        public static void LogError(Exception ex)
        {
            EventLog.WriteEntry(AppConfig.EventLogSource, ex.Message, EventLogEntryType.Error);
        }
    }
}
