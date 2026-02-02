using System;
using Abp.Dependency;
using Abp.Timing;
using Castle.Core.Logging;

namespace inzibackend.Migrations.Seed
{
    public class Log : ITransientDependency
    {
        public ILogger Logger { get; set; }
        public bool ToConsole { get; set; } = false;
        public bool ToFile { get; set; } = true;
        public Log()
        {
            Logger = NullLogger.Instance;
        }

        public void Write(string text)
        {
            if (ToConsole)
                Console.WriteLine(Clock.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + text);
            if (ToFile)
            Logger.Info(text);
        }
    }
}