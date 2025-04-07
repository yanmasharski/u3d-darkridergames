using System;
using Newtonsoft.Json;

public class DataSerializerNewtonsoft : IDataSerializer
{
    public string Serialize<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public T Deserialize<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public object Deserialize(Type type, string data)
    {
        return JsonConvert.DeserializeObject(data, type);
    }

    public string Serialize(Type type, object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }
}