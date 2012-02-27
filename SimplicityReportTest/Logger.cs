using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplicityReportTest
{
    public static class Logger
    {
        public static void LogInfoMessage(string message)
        {
            Log log = new Log();
            log.LogTime = DateTime.Now;
            log.Message = message;
            log.Type = "INFO";
            //LoggingEntities db = new LoggingEntities();
            //db.AddToLogs(log);
            //db.SaveChanges();
        }
    }
}