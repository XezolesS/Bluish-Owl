using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BluishOwl.Logging
{
    internal static class LogSaver
    {
        private static readonly string LogDirectory = Directory.GetCurrentDirectory() + @"\" + "Logs";
        private static string LogPath;
        private static bool LogStarted;

        internal static void SaveLog(string message)
        {
            if (!LogStarted)
            {
                LogPath = BuildLogFilePath();
                LogStarted = true;
            }

            using (StreamWriter sw = new StreamWriter(LogPath, true, Encoding.UTF8))
            {
                sw.Write(message + "\n");
            }
        }

        private static string BuildLogFilePath()
        {
            // If folder "Logs" is not exists, then create new one.
            if (!Directory.Exists(LogDirectory))
                Directory.CreateDirectory(LogDirectory);

            string date = DateTime.Now.ToString("yyyy-MM-dd");
            int i = 0;

            // Create a numbering for file name.
            foreach (string file in Directory.GetFiles(LogDirectory))
                if (file.Contains(date))
                    i++;

            // FORMAT: .../2099-12-31_0.log
            return $"{LogDirectory}\\{date}_{i}.log";
        }
    }
}
