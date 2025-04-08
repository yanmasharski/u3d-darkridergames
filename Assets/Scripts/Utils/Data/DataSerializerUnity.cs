using System;
using UnityEngine;

public class DataSerializerUnity: IDataSerializer
{
    public string Serialize<T>(T obj)
    {
        return JsonUtility.ToJson(obj);
    }

    public T Deserialize<T>(string data)
    {
        return JsonUtility.FromJson<T>(data);   
    }

    public object Deserialize(Type type, string data)
    {
        return JsonUtility.FromJson(data, type);
    }

    public string Serialize(Type type, object obj)
    {
        return JsonUtility.ToJson(obj);
    }
}