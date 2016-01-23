#if GOOGLE_ANALYTICS
namespace DRG.Analytics
{
    using System;
    using UnityEngine;

    public class ASGoogleAnalytics : IAnalyticsSystem
    {

        private GoogleAnalyticsV4 GoogleAnalytics;

        public void Init()
        {
            // fill GA
            GoogleAnalytics = UnityEngine.Object.FindObjectOfType<GoogleAnalyticsV4>();

            if (GoogleAnalytics == null)
            {
                throw new Exception("GoogleAnalytics is not setted up!");
            }
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