using UnityEngine;
using System.Collections.Generic;
using DRG.Localization;

[CreateAssetMenu(fileName = "Locale", menuName = "Locale")]
public class LocaleAsset : ScriptableObject, ILocale
{
    [SerializeField]
    private Dictionary<string, string> dictionary = new Dictionary<string, string>();

    public bool Has(string key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool TryToGet(string key, out string result)
    {
        return dictionary.TryGetValue(key, out result);
    }
}
