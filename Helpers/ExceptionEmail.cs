using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace Helpers
{
    public class ExceptionEmail
    {
        public string Subject { get; set; }
        public List<KeyValuePair<string, string>> ExtraData { get; set; }
        public Exception ex { get; set; }

        #region Send
        public void Send()
        {
            if (ConfigurationManager.AppSettings[ConfigKeys.ShouldSendExceptionEmails] == "Y" && ex != null)
            {
                string body =
                    "<strong>Exception Type:</strong> " + ex.GetType().ToString() + "<br>" +
                    "<br><strong>Exception message:</strong> " + ex.Message + "<br>" +
                    "<br><strong>Source:</strong> " + ex.Source + "<br>" +
                    "<br><strong>Stacktrace:</strong> " + PrettyStackTraceAsHtml(ex) + "<br>";

                if (ExtraData != null)
                {
                    foreach (var kvp in ExtraData)
                    {
                        body = body + "<br><strong>" + kvp.Key + ":</strong> " + kvp.Value + "<br>";
                    }
                }

                body = body + GetInnerExceptionDetailsAsHtml(ex.InnerException);

                try
                {
                    using (MailMessage msg = new MailMessage(
                            ConfigurationManager.AppSettings[ConfigKeys.SendExceptionEmailsFrom],
                            ConfigurationManager.AppSettings[ConfigKeys.SendExceptionEmailsTo],
                            string.IsNullOrWhiteSpace(Subject) ? "[CCCP] Unhandled Exception" : Subject,
                            body))
                    {
                        msg.IsBodyHtml = true;
                        using (SmtpClient mailClient = new SmtpClient(ConfigurationManager.AppSettings[ConfigKeys.MailServer]))
                        {                            
                            mailClient.Send(msg);
                        }
                    }
                }
                catch
                {
                    Logger.LogEvent("Trouble Sending Exception Email: " + ex.GetType() + ": " + ex.Message);
                }
            }
        }
        #endregion

        #region GetInnerExceptionDetails
        private string GetInnerExceptionDetailsAsHtml(Exception ex)
        {
            string details = "";
            if (ex != null)
            {
                details = details +
                        "<br><strong>Inner Exception Type:</strong> " + ex.GetType().ToString() + "<br>" +
                        "<br><strong>Inner Exception Message:</strong> " + ex.Message + "<br>" +
                        "<br><strong>Inner Exception Stacktrace:</strong> " + PrettyStackTraceAsHtml(ex) + "<br>";

                details = details + GetInnerExceptionDetailsAsHtml(ex.InnerException);
            }
            return details;
        }
        #endregion

        #region PrettyStackTraceAsHtml
        private string PrettyStackTraceAsHtml(Exception ex)
        {
            var frames = (" " + ex.StackTrace).Split(new string[] { " at " }, StringSplitOptions.RemoveEmptyEntries);
            var trace = "";
            foreach (var frame in frames)
            {
                trace = trace + "<br>" + frame;
            }
            return trace;
        }
        #endregion
    }
}
