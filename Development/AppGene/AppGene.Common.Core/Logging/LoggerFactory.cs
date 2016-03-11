using System.Diagnostics;

namespace AppGene.Common.Core.Logging
{
    public class LoggerFactory
    {
        public static ILogger GetLogger(string name)
        {
            return new Logger(name);
        }

        public static ILogger GetLogger()
        {
            return new Logger(new StackFrame(1).GetMethod().ReflectedType.FullName);
        }
    }
}
