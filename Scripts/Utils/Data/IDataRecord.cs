/// <summary>
/// Interface for data records that store and manage persistent values.
/// Provides a common contract for different types of data records used by DataStorage.
/// </summary>
/// <typeparam name="T">The type of value stored in the record.</typeparam>
public interface IDataRecord<T> 
{
    /// <summary>
    /// Gets the unique key used to identify this record in storage.
    /// </summary>
    string key { get; }
    
    /// <summary>
    /// Gets whether this record has a value set.
    /// </summary>
    bool hasValue { get; }
    
    /// <summary>
    /// Gets whether this record has been modified since the last save.
    /// </summary>
    bool isDirty { get; }
    
    /// <summary>
    /// Gets whether this record has been fully processed during save operations.
    /// Primarily used for object serialization tracking.
    /// </summary>
    bool processed { get; }
    
    /// <summary>
    /// Sets the value for this record and marks it as dirty.
    /// </summary>
    /// <param name="value">The value to store in the record.</param>
    void SetValue(T value);
    
    /// <summary>
    /// Gets the current value of this record.
    /// </summary>
    /// <returns>The stored value.</returns>
    T GetValue();

    /// <summary>
    /// Applies any pending changes to the underlying storage system.
    /// </summary>
    void Apply();
    
    /// <summary>
    /// Deletes this record from the storage system.
    /// </summary>
    void Delete();
}
