using UnityEngine;

public sealed class DataRecordFloat : IDataRecord<float>
{
    private readonly string keyCache;
    private float value;
    private bool hasValueCache;

    public DataRecordFloat(string key, float defaultValue)
    {
        keyCache = key;
        value = PlayerPrefs.GetFloat(key, defaultValue);
        hasValueCache = PlayerPrefs.HasKey(key);
    }

    public string key => keyCache;

    public bool hasValue => hasValueCache;

    public bool isDirty { get; private set; }

    /// <summary>
    /// Gets whether this record has been fully processed during save operations.
    /// Primarily used for object serialization tracking.
    /// Always returns true for float records.
    /// </summary>
    public bool processed => true;

    public void SetValue(float value)
    {
        if (this.value != value)
        {
            isDirty = true;
        }
        this.value = value;
        hasValueCache = true;
    }

    public float GetValue()
    {
        return value;
    }

    public void Apply()
    {
        if (isDirty)
        {
            PlayerPrefs.SetFloat(key, value);
            isDirty = false;
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(key);
        hasValueCache = false;
    }
}
