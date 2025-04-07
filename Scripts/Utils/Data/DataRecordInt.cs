using UnityEngine;

public sealed class DataRecordInt : IDataRecord<int>
{
    private readonly string keyCache;
    private int value;
    private bool hasValueCache;

    public DataRecordInt(string key, int defaultValue)
    {
        keyCache = key;
        value = PlayerPrefs.GetInt(key, defaultValue);
        hasValueCache = PlayerPrefs.HasKey(key);
    }

    public string key => keyCache;

    public bool hasValue => hasValueCache;

    public bool isDirty { get; private set; }

    /// <summary>
    /// Gets whether this record has been fully processed during save operations.
    /// Primarily used for object serialization tracking.
    /// Always returns true for int records.
    /// </summary>
    public bool processed => true;

    public void SetValue(int value)
    {
        if (this.value != value)
        {
            isDirty = true;
        }
        this.value = value;
        hasValueCache = true;
    }

    public int GetValue()
    {
        return value;
    }

    public void Apply()
    {
        if (isDirty)
        {
            PlayerPrefs.SetInt(key, value);
            isDirty = false;
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(key);
        hasValueCache = false;
    }
}