#if UNITY_ANALYTICS
namespace DRG.Analytics
{
    using System.Collections.Generic;
    using UnityEngine.Analytics;

    public class ASUnityAnalytics : IAnalyticsSystem
    {
        private Dictionary<string, object> EmptyData = new Dictionary<string, object>();

        public void Init()
        {

        }

        public void SendEvent(string name)
        {
            UnityEngine.Analytics.Analytics.CustomEvent(name, EmptyData);
        }

    }
}
#endif