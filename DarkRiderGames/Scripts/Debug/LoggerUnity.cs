namespace DRG.Debug
{
    public class LoggerUnity : ILogger
    {
        public void Message(string m)
        {
            UnityEngine.Debug.Log(m);
        }

        public void Warning(string m)
        {
            UnityEngine.Debug.LogWarning(m);
        }

        public void Error(string m)
        {
            UnityEngine.Debug.LogError(m);
        }
    }
}
