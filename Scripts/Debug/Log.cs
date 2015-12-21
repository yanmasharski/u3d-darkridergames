namespace DRG.Debug
{
    public static class Log
    {
        private static ILogger Logger = new LoggerMockup();

        public static void Setup(ILogger logger)
        {
            Logger = logger;
        }

        public static void Message(string m)
        {
            Logger.Message(m);
        }

        public static void Warning(string m)
        {
            Logger.Warning(m);
        }

        public static void Error(string m)
        {
            Logger.Error(m);
        }

        public static void Exception(System.Exception e)
        {
            Logger.Exception(e);
        }
    }
}