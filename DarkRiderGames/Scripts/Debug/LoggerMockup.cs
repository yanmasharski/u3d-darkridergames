namespace DRG.Debug
{
    public class LoggerMockup : ILogger
    {
        public void Message(string m)
        {
            // NOOP
        }

        public void Warning(string m)
        {
            // NOOP
        }

        public void Error(string m)
        {
            // NOOP
        }

        public void Exception(System.Exception e)
        {
            // NOOP
        }
    }
}