namespace DRG.Analytics
{
    public static class AnalyticsAutostart
    {

        public static void Start()
        {
#if UNITY_ANALYTICS
            Analytics.Connect(new ASUnityAnalytics());
#endif

#if GAME_ANALYTICS
            Analytics.Connect(new ASGameAnalytics());
#endif

#if GOOGLE_ANALYTICS
            Analytics.Connect(new ASGoogleAnalytics());
#endif
        }
    }
}
