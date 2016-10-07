using System;
using System.IO;

namespace BPA_RPG
{
    public class EventLogger
    {
        private readonly StreamWriter file;

        public EventLogger()
        {
            if (!Directory.Exists("Logs"))
                Directory.CreateDirectory("Logs");

            file = File.CreateText("Logs/log_" + string.Format("{0:yyyy-MM-dd_HH-mm-ss}", DateTime.Now) + ".txt");
        }

        /// <summary>
        /// Logs important data to file
        /// </summary>
        /// <param name="type">Sender's class type</param>
        /// <param name="log">String to log to file</param>
        public void Log(Type type, string log)
        {
            file.WriteLine("[" + string.Format("{0:HH:mm:ss.ff}", DateTime.Now) + "] [" + type.ToString() + "] " + log);
            file.Flush();
        }

        /// <summary>
        /// Logs important data to file
        /// </summary>
        /// <param name="sender">Object log is being sent from</param>
        /// <param name="log">String to log to file</param>
        public void Log(object sender, string log)
        {
            Log(sender.GetType(), log);
        }
    }
}
