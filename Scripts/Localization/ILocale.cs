namespace DRG.Localization
{
    public interface ILocale
    {
        bool Has(string key);

        bool TryToGet(string key, out string result);
    }
}