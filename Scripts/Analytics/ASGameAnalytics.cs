#if GAME_ANALYTICS
namespace DRG.Analytics
{
    using using GameAnalyticsSDK;

    public class ASGameAnalytics : IAnalyticsSystem
    {

        public void Init()
        {

        }

        public void SendEvent(string name)
        {
            GameAnalytics.NewDesignEvent(eventName);
        }

    }
}
#endif