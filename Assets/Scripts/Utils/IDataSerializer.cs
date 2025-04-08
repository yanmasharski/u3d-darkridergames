using System;

/// <summary>
/// Interface for data serialization and deserialization operations.
/// Provides a common contract for converting objects to and from string representations.
/// </summary>
public interface IDataSerializer
{
    /// <summary>
    /// Serializes an object of type T to a string representation.
    /// </summary>
    /// <typeparam name="T">The type of object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A string representation of the serialized object.</returns>
    string Serialize<T>(T obj);
    
    /// <summary>
    /// Deserializes a string representation back into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the data into.</typeparam>
    /// <param name="data">The string data to deserialize.</param>
    /// <returns>The deserialized object of type T.</returns>
    T Deserialize<T>(string data);
    
    /// <summary>
    /// Deserializes a string representation into an object of the specified type.
    /// </summary>
    /// <param name="type">The type to deserialize the data into.</param>
    /// <param name="data">The string data to deserialize.</param>
    /// <returns>The deserialized object as a generic object reference.</returns>
    object Deserialize(Type type, string data);
    
    /// <summary>
    /// Serializes an object of the specified type to a string representation.
    /// </summary>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A string representation of the serialized object.</returns>
    string Serialize(Type type, object obj);
}