/// <summary>
/// Interface for data records that store and manage persistent values.
/// Provides a common contract for different types of data records used by DataStorage.
/// </summary>
/// <typeparam name="T">The type of value stored in the record.</typeparam>
public interface IDataRecord<T>: IDataRecordBase
{
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

}
