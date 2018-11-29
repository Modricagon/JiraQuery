using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace JiraQuery
{
    public static class Log
    {
        private static bool externalOutput = true;
        private static bool consoleOutput = true;
        private static string logName = "";

        public static void LogMessage(string message)
        {
            string contents = Time() + message;
            if (consoleOutput) Console.WriteLine(contents);
            if (externalOutput) LogExternal(contents);
            
        }

        public static void LogMessage(string message, bool NewLine)
        {
            if (consoleOutput) Console.WriteLine("\n" + Time() + message);

        }

        private static string Time()
        {
            DateTime Time = DateTime.Now;
            return (string.Format("[{0}][{1}] ", Time.ToShortDateString(), Time.ToLongTimeString()));
        }

        private static void LogExternal(string message) //external log 
        {
            File.AppendAllText(string.Format("log {0}.txt", logName), message + Environment.NewLine);
        }

        private static void LogExternal(List<string> messages)
        {
            string content = "";
            foreach (string message in messages)
            {
                content = content + message + "\n";
            }
            File.AppendAllText(string.Format("log {0}.txt", logName), content + Environment.NewLine);
        }

        public static string LogName //get and set for logName
        {
            get { return logName; }
            set
            {
                logName = value;
            }
        }

        public static bool ExternalOutput //get and set for externalOutput
        {
            get { return externalOutput; }
            set
            {
                externalOutput = value;
            }
        }

        public static bool ConsoleOutput //get and set for consoleOutput
        {
            set
            {
                consoleOutput = value;
            }
        }
    }
}
