using log4net;

namespace GenderPrediction.Models
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warn = 2,
        Error = 3,
        Fatal = 4
    }

    public static  class Logger
    {
        public static string Ip;
        public static int ReqCount=0;
        public static void LogMsg( LogLevel level, object msg)
        {
           
            GlobalContext.Properties["LogFileName"] = "Data.log";
            log4net.Config.XmlConfigurator.Configure();

            ILog log = LogManager.GetLogger(typeof(Logger));

            switch (level)
            {
                case LogLevel.Debug:
                    log.Debug(msg);
                    break;

                case LogLevel.Info:
                    log.Info(msg);
                    break;

                case LogLevel.Warn:
                    log.Warn(msg);
                    break;

                case LogLevel.Error:
                    log.Error(msg);
                    break;

                case LogLevel.Fatal:
                    log.Fatal(msg);
                    break;
            }
        }
    }
}