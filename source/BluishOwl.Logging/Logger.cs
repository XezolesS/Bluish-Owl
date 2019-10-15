using Discord;
using System;

namespace BluishOwl.Logging
{
    public static class Logger
    {
        public static void Log(LogSeverity severity, string source, string value)
        {
            string message = FormatLogMessage(source, value);
            SendMessage(severity, message);
            LogSaver.SaveLog(message);
        }

        public static void Log(LogSeverity severity, string source, string format, params object[] args)
        {
            string message = FormatLogMessage(source, format, args);
            SendMessage(severity, message);
            LogSaver.SaveLog(message);
        }

        public static void Critical(string source, string value) => Log(LogSeverity.Critical, source, value);
        public static void Critical(string source, string format, params object[] args) => Log(LogSeverity.Critical, source, format, args);

        public static void Debug(string source, string value) => Log(LogSeverity.Debug, source, value);
        public static void Debug(string source, string format, params object[] args) => Log(LogSeverity.Debug, source, format, args);

        public static void Error(string source, string value) => Log(LogSeverity.Error, source, value);
        public static void Error(string source, string format, params object[] args) => Log(LogSeverity.Error, source, format, args);

        public static void Info(string source, string value) => Log(LogSeverity.Info, source, value);
        public static void Info(string source, string format, params object[] args) => Log(LogSeverity.Info, source, format, args);

        public static void Verbose(string source, string value) => Log(LogSeverity.Verbose, source, value);
        public static void Verbose(string source, string format, params object[] args) => Log(LogSeverity.Verbose, source, format, args);

        public static void Warning(string source, string value) => Log(LogSeverity.Warning, source, value);
        public static void Warning(string source, string format, params object[] args) => Log(LogSeverity.Warning, source, format, args);

        private static void SetConsoleForegroundColorByLogSeverity(LogSeverity severity)
        {
            switch (severity)
            {
                case LogSeverity.Critical: Console.ForegroundColor = ConsoleColor.DarkRed; break;
                case LogSeverity.Debug: Console.ForegroundColor = ConsoleColor.Magenta; break;
                case LogSeverity.Error: Console.ForegroundColor = ConsoleColor.Red; break;
                case LogSeverity.Info: Console.ForegroundColor = ConsoleColor.White; break;
                case LogSeverity.Verbose: Console.ForegroundColor = ConsoleColor.Blue; break;
                case LogSeverity.Warning: Console.ForegroundColor = ConsoleColor.Yellow; break;
            }
        }

        private static string FormatLogMessage(string source, string value)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")}] [{source}]\t {value}";
        }

        private static string FormatLogMessage(string source, string format, params object[] args)
        {
            return $"[{DateTime.Now.ToString("HH:mm:ss")}] [{source}]\t {string.Format(format, args)}";
        }

        private static void SendMessage(LogSeverity severity, string message)
        {
            SetConsoleForegroundColorByLogSeverity(severity);
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
