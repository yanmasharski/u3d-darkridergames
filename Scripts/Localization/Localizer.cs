namespace DRG.Localization
{
    using System.Collections.Generic;

    public static class Localizer
    {
        private static Dictionary<string, string> DictionaryActive = new Dictionary<string, string>();

        public static string GetValue(string key)
        {
            string val;

            if (DictionaryActive.TryGetValue(key, out val) == false)
            {
                return key;
            }

            return val;
        }
    }
}