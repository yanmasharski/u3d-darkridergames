using UnityEngine;

public sealed class DataRecordString : IDataRecord<string>
{
    private readonly string keyCache;
    private string value;
    private bool hasValueCache;

    public DataRecordString(string key, string defaultValue)
    {
        keyCache = key;
        value = PlayerPrefs.GetString(key, defaultValue);
        hasValueCache = PlayerPrefs.HasKey(key);
    }
    public string key => keyCache;
    public bool hasValue => hasValueCache;

    public bool isDirty { get; private set; }

    /// <summary>
    /// Gets whether this record has been fully processed during save operations.
    /// Primarily used for object serialization tracking.
    /// Always returns true for string records.
    /// </summary>
    public bool processed => true;

    public void SetValue(string value)
    {
        if (this.value != value)
        {
            isDirty = true;
        }
        this.value = value;
        hasValueCache = true;
    }

    public string GetValue()
    {
        return value;
    }

    public void Apply()
    {
        if (isDirty)
        {
            PlayerPrefs.SetString(key, value);
            isDirty = false;

            if (value.Length > 20000)
            {
                Debug.LogWarning($"Potential ANR warning! String with length {value.Length} is too large for {key}.");
            }
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(key);
        hasValueCache = false;
    }
}
