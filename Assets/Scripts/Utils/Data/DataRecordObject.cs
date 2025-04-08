using System;
using UnityEngine;

public sealed class DataRecordObject : IDataRecord<object>
{
    private readonly string keyCache;
    private readonly IDataSerializer serializer;
    private readonly Type type;
    private object value;
    private bool hasValueCache;
    public DataRecordObject(string key, Type type, object defaultValue, IDataSerializer serializer)
    {
        keyCache = key;
        this.type = type;
        this.serializer = serializer;
        hasValueCache = PlayerPrefs.HasKey(key);
        value = hasValueCache ? serializer.Deserialize(type, PlayerPrefs.GetString(key)) : defaultValue;
    }

    public string key => keyCache;
    public bool hasValue => hasValueCache;

    public bool isDirty { get; private set; }

    public bool processed { get; private set; }

    public void SetValue(object value)
    {
        if (!this.value.Equals(value))
        {
            isDirty = true;
        }

        this.value = value;
        hasValueCache = true;
        processed = false;
    }

    public object GetValue()
    {
        return value;
    }

    public T GetValue<T>()
    {
        return (T)value;
    }

    public void Apply()
    {
        if (!isDirty)
        {
            return;
        }

        System.Threading.Tasks.Task.Run(SerializationTask);

        void SerializationTask()
        {
            string serializedData = serializer.Serialize(value);
            MainThreadDispatcher.Enqueue(OnMainThread);
            void OnMainThread()
            {
                PlayerPrefs.SetString(key, serializedData);
                processed = true;
            }
        }
    }

    public void Delete()
    {
        PlayerPrefs.DeleteKey(key);
        hasValueCache = false;
    }
}
