using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace GenderPrediction.Models
{
    public enum LogLevel
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        ERROR = 3,
        FATAL = 4
    }

    public static  class Logger
    {
        public static string ip;
        public static int reqCount=0;
        public static void LogMsg( LogLevel level, object msg)
        {
           
            log4net.GlobalContext.Properties["LogFileName"] = "Data.log";
            log4net.Config.XmlConfigurator.Configure();

            ILog log = LogManager.GetLogger(typeof(Logger));

            switch (level)
            {
                case LogLevel.DEBUG:
                    log.Debug(msg);
                    break;

                case LogLevel.INFO:
                    log.Info(msg);
                    break;

                case LogLevel.WARN:
                    log.Warn(msg);
                    break;

                case LogLevel.ERROR:
                    log.Error(msg);
                    break;

                case LogLevel.FATAL:
                    log.Fatal(msg);
                    break;
            }
        }
    }
}