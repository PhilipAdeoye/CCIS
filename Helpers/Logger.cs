using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;

namespace Helpers
{
    public class Logger
    {
        private static string LogFileDirectory = ConfigurationManager.AppSettings[ConfigKeys.LogFileDirectory];

        #region LogEvent
        public static void LogEvent(string message)
        {
            try
            {
                using (var sw = File.AppendText(GetLogFileName()))
                {
                    sw.WriteLine(Environment.NewLine + DateTime.Now.ToString() + ": " + message);
                }
            }
            catch (Exception) { }
        } 
        #endregion

        #region LogException
        public static void LogException(Exception ex, string message = "")
        {
            if (ConfigurationManager.AppSettings[ConfigKeys.ShouldLogExceptions] == "Y")
            {
                try
                {
                    using (var sw = File.AppendText(GetLogFileName()))
                    {
                        sw.WriteLine(Environment.NewLine + DateTime.Now.ToString() + ": " + message);
                        sw.WriteLine("Exception Type: " + ex.GetType().ToString());
                        sw.WriteLine("Exception Message: " + ex.Message);
                        sw.WriteLine("Stacktrace: " + ex.StackTrace);
                        sw.Write(GetInnerExceptionDetails(ex.InnerException));
                    }
                }
                catch (Exception) { }
            }
        } 
        #endregion

        #region GetLogFileName
        private static string GetLogFileName()
        {
            if (!Directory.Exists(LogFileDirectory))
                Directory.CreateDirectory(LogFileDirectory);

            return Path.Combine(LogFileDirectory, DateTime.Today.ToString("yyyy-MM-dd") + ".txt");
        } 
        #endregion

        #region GetInnerExceptionDetails
        private static string GetInnerExceptionDetails(Exception ex)
        {
            string details = "";
            if (ex != null)
            {
                details = details +
                        Environment.NewLine + "Inner Exception Type: " + ex.GetType().ToString() +
                        Environment.NewLine + "Inner Exception Message: " + ex.Message +
                        Environment.NewLine + "Inner Exception Stacktrace: " + ex.StackTrace;

                details = details + GetInnerExceptionDetails(ex.InnerException);
            }
            return details;
        } 
        #endregion
    }
}
