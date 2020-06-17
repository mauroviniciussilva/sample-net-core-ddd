using NLog;
using NLog.Config;
using NLog.Targets;

namespace Sample.Infra.Logging
{
    public static class LogConfigurationFactory
    {
        public static LoggingConfiguration CreateConfiguration(LoggingLayoutTemplate layout)
        {
            var configuraion = new LoggingConfiguration();

            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/logs/erros.log",
                Layout = layout.ToString(),
                CreateDirs = true,
                MaxArchiveFiles = 15,
                ArchiveEvery = FileArchivePeriod.Day,
                ConcurrentWrites = false,
                ArchiveFileName = "${basedir}/logs/erros.arquivado.{#}.log",
                ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
                ArchiveDateFormat = "dd-MM-yyyy",
                ArchiveAboveSize = 1000 * 1000 * 15 //15mb
            };

            var consoleTarget = new ColoredConsoleTarget
            {
                Layout = "${longdate} - ${level:uppercase=true} - ${message}"
            };

            configuraion.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, consoleTarget));
            configuraion.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, fileTarget));

            return configuraion;
        }
    }
}