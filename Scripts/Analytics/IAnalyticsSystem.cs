namespace DRG.Analytics
{
    public interface IAnalyticsSystem
    {
        void Init();
        void SendEvent(string name);
    }
}
