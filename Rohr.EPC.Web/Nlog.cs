using NLog;
using NLog.Config;
using NLog.Targets;

namespace Rohr.EPC.Web
{
    public static class NLog
    {
        public static Logger Log()
        {
            LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
            FileTarget filerTarget = new FileTarget
            {
                Layout = @"${level};${date};${message};${callsite};${stacktrace};",
                FileName = @"${basedir}/logs/${shortdate}.log"
            };

            loggingConfiguration.AddTarget("file", filerTarget);

            LoggingRule loggingRuleDebug = new LoggingRule("*", LogLevel.Error, filerTarget);
            loggingConfiguration.LoggingRules.Add(loggingRuleDebug);

            LogManager.Configuration = loggingConfiguration;

            return LogManager.GetCurrentClassLogger();
        }
    }
}