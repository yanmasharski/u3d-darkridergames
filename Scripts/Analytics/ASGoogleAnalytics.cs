#if GOOGLE_ANALYTICS
namespace DRG.Analytics
{
    using UnityEngine;

    public class ASGoogleAnalytics : IAnalyticsSystem
    {

        private GoogleAnalyticsV4 GoogleAnalytics;

        public void Init()
        {
            // fill GA
            GoogleAnalytics = new GameObject("GoogleAnalytics").AddComponent<GoogleAnalyticsV4>();
            Object.DontDestroyOnLoad(GoogleAnalytics.gameObject);
        }

        public void SendEvent(string name)
        {
            EventHitBuilder ehb = new EventHitBuilder();
            ehb.SetEventAction(name);
            GoogleAnalytics.LogEvent(ehb);
        }

    }
}
#endif