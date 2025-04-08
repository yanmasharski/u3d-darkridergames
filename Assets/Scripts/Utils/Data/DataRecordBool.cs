using UnityEngine;

public sealed class DataRecordBool : IDataRecord<bool>
{
    private readonly string keyCache;
    private bool value;
    private bool hasValueCache;

    public DataRecordBool(string key, bool defaultValue)
    {
        keyCache = key;
        value = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
        hasValueCache = PlayerPrefs.HasKey(key);
    }

    public string key => keyCache;
    public bool hasValue => hasValueCache;

    public bool isDirty { get; private set; }

    /// <summary>
    /// Gets whether this record has been fully processed during save operations.
    /// Primarily used for object serialization tracking.
    /// Always returns true for bool records.
    /// </summary>
    public bool processed => true;

    public void SetValue(bool value)
    {
        if (this.value != value)
        {
            isDirty = true;
        }
        this.value = value;
        hasValueCache = true;
    }

    public bool GetValue()
    {
        return value;
    }

    public void Apply()
    {
        if (isDirty)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            isDirty = false;
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(key);
        hasValueCache = false;
    }
}
