namespace DRG.Localization
{
    using System.Collections.Generic;

    public static class Localizer
    {
        private static List<ILocale> localeList = new List<ILocale>();

        public static string Get(string key)
        {
            string val;

            for (int i = 0; i < localeList.Count; i++)
            {
                if (localeList[i].TryToGet(key, out val))
                {
                    return val;
                }
            }

#if UNITY_EDITOR
            return "#" + key;
#else
            return key;
#endif
        }

        public static void AddLocale(ILocale locale)
        {

        }
    }
}