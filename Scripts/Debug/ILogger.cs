public interface ILogger
{
    void Message(string m);
    void Warning(string m);
    void Error(string m);
    void Exception(System.Exception e);
}
