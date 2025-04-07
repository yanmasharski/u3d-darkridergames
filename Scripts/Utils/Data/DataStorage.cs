using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataStorage is a high-performance wrapper around Unity's PlayerPrefs system.
/// It provides an in-memory cache with dirty tracking to minimize disk operations
/// and improve performance when dealing with persistent data.
/// Additionally, this system enforces structured data access through the DataRecord
/// interface, which enhances code organization, facilitates debugging, and enables
/// robust data validation.
/// </summary>
public static class DataStorage
{
    private static readonly RecordsStorage recordsStorage = new RecordsStorage();
    private static SaveProcessor saveProcessor;
    private static Coroutine saveCoroutine;

    static DataStorage()
    {
#if UNITY_IOS
        Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
#endif

        saveProcessor = new GameObject("SaveProcessor").AddComponent<SaveProcessor>();
        GameObject.DontDestroyOnLoad(saveProcessor.gameObject);
    }

    /// <summary>
    /// Checks if a key exists in any of the data records or in PlayerPrefs.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the key exists, false otherwise.</returns>
    public static bool HasKey(string key)
    {
        bool hasKey = false;

        lock (recordsStorage.lockObject)
        {
            hasKey = recordsStorage.ContainsKey(key);
        }

        hasKey |= PlayerPrefs.HasKey(key);

        return hasKey;
    }

    /// <summary>
    /// Gets an integer value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored integer value record.</returns>
    public static DataRecordInt GetInt(string key, int defaultVal = 0)
    {
        lock (recordsStorage.lockObject)
        {
            if (!recordsStorage.recordsInt.TryGetValue(key, out var result))
            {
                result = new DataRecordInt(key, defaultVal);
                recordsStorage.recordsInt[key] = result;
            }

            return result;
        }
    }

    /// <summary>
    /// Gets a float value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored float value record.</returns>
    public static DataRecordFloat GetFloat(string key, float defaultVal = 0f)
    {
        lock (recordsStorage.lockObject)
        {
            if (!recordsStorage.recordsFloat.TryGetValue(key, out var result))
            {
                result = new DataRecordFloat(key, defaultVal);
                recordsStorage.recordsFloat[key] = result;
            }

            return result;
        }
    }

    /// <summary>
    /// Gets a string value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored string value record.</returns>
    public static DataRecordString GetString(string key, string defaultVal = "")
    {
        lock (recordsStorage.lockObject)
        {
            if (!recordsStorage.recordsString.TryGetValue(key, out var result))
            {
                result = new DataRecordString(key, defaultVal);
                recordsStorage.recordsString[key] = result;
            }

            return result;
        }
    }

    /// <summary>
    /// Gets an object value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve.</typeparam>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="serializer">The serializer to use for converting from the stored format.</param>
    /// <returns>The stored object record.</returns>
    public static DataRecordObject GetObject<T>(string key, IDataSerializer serializer)
    {
        lock (recordsStorage.lockObject)
        {
            if (!recordsStorage.recordsObject.TryGetValue(key, out var result))
            {
                result = new DataRecordObject(key, typeof(T), default, serializer);
                recordsStorage.recordsObject[key] = result;
            }

            return result;
        }
    }

    /// <summary>
    /// Gets a boolean value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored boolean value record.</returns>
    public static DataRecordBool GetBool(string key, bool defaultValue = default)
    {
        lock (recordsStorage.lockObject)
        {
            if (!recordsStorage.recordsBool.TryGetValue(key, out var result))
            {
                result = new DataRecordBool(key, defaultValue);
                recordsStorage.recordsBool[key] = result;
            }

            return result;
        }
    }

    /// <summary>
    /// Saves all dirty records to PlayerPrefs. Can be delayed by specifying a frame cooldown.
    /// </summary>
    /// <param name="framesCooldown">Number of frames to wait before saving. If greater than 0,
    /// the save operation will be performed after the specified number of frames.</param>
    public static void Save(int framesCooldown = 60)
    {
        saveProcessor.framesCooldown = framesCooldown;
        if (saveCoroutine != null)
        {
            saveProcessor.StopCoroutine(saveCoroutine);
        }

        saveCoroutine = saveProcessor.StartCoroutine(saveProcessor.SaveCoroutine(framesCooldown));
    }

    /// <summary>
    /// Deletes a key and its associated value from all records and PlayerPrefs.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    public static void DeleteKey(string key)
    {
        lock (recordsStorage.lockObject)
        {
            if (recordsStorage.TryGetValue(key, out var record))
            {
                record.Delete();
            }
        }
    }

    /// <summary>
    /// Deletes all keys and values from all records and PlayerPrefs.
    /// </summary>
    public static void DeleteAll()
    {
        lock (recordsStorage.lockObject)
        {
            foreach (var record in recordsStorage)
            {
                record.Delete();
            }

            recordsStorage.Clear();
        }
    }

    private class RecordsStorage : IEnumerable<IDataRecordBase>
    {
        public readonly Dictionary<string, DataRecordInt> recordsInt = new Dictionary<string, DataRecordInt>();
        public readonly Dictionary<string, DataRecordBool> recordsBool = new Dictionary<string, DataRecordBool>();
        public readonly Dictionary<string, DataRecordFloat> recordsFloat = new Dictionary<string, DataRecordFloat>();
        public readonly Dictionary<string, DataRecordString> recordsString = new Dictionary<string, DataRecordString>();
        public readonly Dictionary<string, DataRecordObject> recordsObject = new Dictionary<string, DataRecordObject>();
        public readonly object lockObject = new object();

        public IEnumerator<IDataRecordBase> GetEnumerator()
        {
            foreach (var record in recordsInt)
            {
                yield return record.Value;
            }

            foreach (var record in recordsBool)
            {
                yield return record.Value;
            }

            foreach (var record in recordsFloat)
            {
                yield return record.Value;
            }

            foreach (var record in recordsString)
            {
                yield return record.Value;
            }

            foreach (var record in recordsObject)
            {
                yield return record.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(string key)
        {
            return recordsInt.ContainsKey(key) ||
                recordsBool.ContainsKey(key) ||
                recordsFloat.ContainsKey(key) ||
                recordsString.ContainsKey(key) ||
                recordsObject.ContainsKey(key);
        }

        public bool TryGetValue(string key, out IDataRecordBase record)
        {
            if (recordsInt.TryGetValue(key, out var resultInt))
            {
                record = resultInt;
                return true;
            }

            if (recordsBool.TryGetValue(key, out var resultBool))
            {
                record = resultBool;
                return true;
            }

            if (recordsFloat.TryGetValue(key, out var resultFloat))
            {
                record = resultFloat;
                return true;
            }

            if (recordsString.TryGetValue(key, out var resultString))
            {
                record = resultString;
                return true;
            }

            if (recordsObject.TryGetValue(key, out var resultObject))
            {
                record = resultObject;
                return true;
            }

            record = null;
            return false;
        }

        public void Clear()
        {
            recordsInt.Clear();
            recordsBool.Clear();
            recordsFloat.Clear();
            recordsString.Clear();
            recordsObject.Clear();
        }
    }

    /// <summary>
    /// Internal MonoBehaviour that handles delayed save operations.
    /// </summary>
    private class SaveProcessor : MonoBehaviour
    {
        public int framesCooldown;

        /// <summary>
        /// Coroutine that waits for the specified number of frames before saving.
        /// </summary>
        /// <param name="framesCooldown">Number of frames to wait before saving.</param>
        /// <returns>IEnumerator for the coroutine.</returns>
        public IEnumerator SaveCoroutine(int framesCooldown)
        {
            this.framesCooldown = framesCooldown;

            while (this.framesCooldown > 0)
            {
                yield return null;
                this.framesCooldown--;
            }

            // Force save all records
            var hasChanges = false;

            lock (recordsStorage.lockObject)
            {
                foreach (var record in recordsStorage)
                {
                    hasChanges |= record.isDirty;
                    record.Apply();
                }
            }

            if (!hasChanges)
            {
                yield break;
            }

            // Wait for all records of objects to be processed
            lock (recordsStorage.lockObject)
            {
                var processed = false;
                while (!processed)
                {
                    foreach (var record in recordsStorage.recordsObject)
                    {
                        processed |= record.Value.processed;
                    }

                    yield return null;
                }
            }

            PlayerPrefs.Save();
            saveCoroutine = null;
        }
    }
}