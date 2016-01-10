namespace DRG.Analytics
{
    using System;
    using System.Collections.Generic;
    using DRG.Debug;

    public static class Analytics
    {
        private static object Lock = new object();
        private static List<IAnalyticsSystem> Systems = new List<IAnalyticsSystem>();

        public static void Connect(IAnalyticsSystem ansys)
        {
            lock (Lock)
            {
                Systems.Add(ansys);
                ansys.Init();
            }
        }

        public static void SendEvent(string name)
        {
            lock (Lock)
            {
                for (int i = 0; i < Systems.Count; i++)
                {
                    try
                    {
                        Systems[i].SendEvent(name);
                    }
                    catch(Exception e)
                    {
                        Log.Exception(e);
                    }
                }
            }
        }
    }
}
