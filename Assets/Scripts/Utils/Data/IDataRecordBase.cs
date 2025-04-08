public interface IDataRecordBase
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
    /// Applies any pending changes to the underlying storage system.
    /// </summary>
    void Apply();
    
    /// <summary>
    /// Deletes this record from the storage system.
    /// </summary>
    void Delete();
}