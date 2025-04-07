using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DataStorage is a high-performance wrapper around Unity's PlayerPrefs system.
/// It provides an in-memory cache with dirty tracking to minimize disk operations
/// and improve performance when dealing with persistent data.
/// </summary>
public static class DataStorage
{
    private static readonly Dictionary<string, DataRecordInt> recordsInt = new Dictionary<string, DataRecordInt>();
    private static readonly Dictionary<string, DataRecordBool> recordsBool = new Dictionary<string, DataRecordBool>();
    private static readonly Dictionary<string, DataRecordFloat> recordsFloat = new Dictionary<string, DataRecordFloat>();
    private static readonly Dictionary<string, DataRecordString> recordsString = new Dictionary<string, DataRecordString>();
    private static readonly Dictionary<string, DataRecordObject> recordsObject = new Dictionary<string, DataRecordObject>();
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

        lock (recordsInt)
        {
            hasKey |= recordsInt.ContainsKey(key);
        }

        if (hasKey)
        {
            return true;
        }

        lock (recordsBool)
        {
            hasKey |= recordsBool.ContainsKey(key);
        }

        if (hasKey)
        {
            return true;
        }

        lock (recordsFloat)
        {
            hasKey |= recordsFloat.ContainsKey(key);
        }

        if (hasKey)
        {
            return true;
        }

        lock (recordsString)
        {
            hasKey |= recordsString.ContainsKey(key);
        }

        if (hasKey)
        {
            return true;
        }

        lock (recordsObject)
        {
            hasKey |= recordsObject.ContainsKey(key);
        }

        if (hasKey)
        {
            return true;
        }

        hasKey |= PlayerPrefs.HasKey(key);

        return hasKey;
    }

    /// <summary>
    /// Sets an integer value for the specified key. The value is cached in memory
    /// and only written to disk when Save() is called.
    /// </summary>
    /// <param name="key">The key to set.</param>
    /// <param name="val">The integer value to store.</param>
    public static void SetInt(string key, int val)
    {
        lock (recordsInt)
        {
            if (!recordsInt.TryGetValue(key, out var result))
            {
                result = new DataRecordInt(key, val);
                recordsInt[key] = result;
            }

            result.SetValue(val);
        }
    }

    /// <summary>
    /// Sets a float value for the specified key. The value is cached in memory
    /// and only written to disk when Save() is called.
    /// </summary>
    /// <param name="key">The key to set.</param>
    /// <param name="val">The float value to store.</param>
    public static void SetFloat(string key, float val)
    {
        lock (recordsFloat)
        {
            if (!recordsFloat.TryGetValue(key, out var result))
            {
                result = new DataRecordFloat(key, val);
                recordsFloat[key] = result;
            }

            result.SetValue(val);
        }
    }

    /// <summary>
    /// Sets a string value for the specified key. The value is cached in memory
    /// and only written to disk when Save() is called. If the value is null or empty,
    /// the key is deleted.
    /// </summary>
    /// <param name="key">The key to set.</param>
    /// <param name="val">The string value to store.</param>
    public static void SetString(string key, string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            DeleteKey(key);
        }
        else
        {
            lock (recordsString)
            {
                if (!recordsString.TryGetValue(key, out var result))
                {
                    result = new DataRecordString(key, val);
                    recordsString[key] = result;
                }

                result.SetValue(val);
            }
        }
    }

    /// <summary>
    /// Sets an object value for the specified key. The object is serialized using the provided
    /// serializer and cached in memory. It's only written to disk when Save() is called.
    /// If the object is null, the key is deleted.
    /// </summary>
    /// <typeparam name="T">The type of object to store.</typeparam>
    /// <param name="key">The key to set.</param>
    /// <param name="obj">The object to store.</param>
    /// <param name="serializer">The serializer to use for converting the object to a storable format.</param>
    public static void SetObject<T>(string key, T obj, IDataSerializer serializer)
    {
        if (obj == null)
        {
            DeleteKey(key);
        }
        else
        {
            lock (recordsObject)
            {
                if (!recordsObject.TryGetValue(key, out var result))
                {
                    result = new DataRecordObject(key, typeof(T), obj, serializer);
                    recordsObject[key] = result;
                }

                result.SetValue(obj);
            }
        }
    }

    /// <summary>
    /// Sets a boolean value for the specified key. The value is cached in memory
    /// and only written to disk when Save() is called.
    /// </summary>
    /// <param name="key">The key to set.</param>
    /// <param name="value">The boolean value to store.</param>
    public static void SetBool(string key, bool value)
    {
        lock (recordsBool)
        {
            if (!recordsBool.TryGetValue(key, out var result))
            {
                result = new DataRecordBool(key, value);
                recordsBool[key] = result;
            }

            result.SetValue(value);
        }
    }

    /// <summary>
    /// Gets an integer value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored integer value or the default value.</returns>
    public static int GetInt(string key, int defaultVal = 0)
    {
        lock (recordsInt)
        {
            if (!recordsInt.TryGetValue(key, out var result))
            {
                result = new DataRecordInt(key, defaultVal);
                recordsInt[key] = result;
            }

            return result.GetValue();
        }
    }

    /// <summary>
    /// Gets a float value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored float value or the default value.</returns>
    public static float GetFloat(string key, float defaultVal = 0f)
    {
        lock (recordsFloat)
        {
            if (!recordsFloat.TryGetValue(key, out var result))
            {
                result = new DataRecordFloat(key, defaultVal);
                recordsFloat[key] = result;
            }

            return result.GetValue();
        }
    }

    /// <summary>
    /// Gets a string value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultVal">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored string value or the default value.</returns>
    public static string GetString(string key, string defaultVal = "")
    {
        lock (recordsString)
        {
            if (!recordsString.TryGetValue(key, out var result))
            {
                result = new DataRecordString(key, defaultVal);
                recordsString[key] = result;
            }

            return result.GetValue();
        }
    }

    /// <summary>
    /// Gets an object value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <typeparam name="T">The type of object to retrieve.</typeparam>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="serializer">The serializer to use for converting from the stored format.</param>
    /// <returns>The stored object or the default value for the type.</returns>
    public static T GetObject<T>(string key, IDataSerializer serializer)
    {
        lock (recordsObject)
        {
            if (!recordsObject.TryGetValue(key, out var result))
            {
                result = new DataRecordObject(key, typeof(T), default, serializer);
                recordsObject[key] = result;
            }

            return result.GetValue<T>();
        }
    }

    /// <summary>
    /// Gets a boolean value for the specified key. Returns the default value if the key doesn't exist.
    /// </summary>
    /// <param name="key">The key to retrieve.</param>
    /// <param name="defaultValue">The default value to return if the key doesn't exist.</param>
    /// <returns>The stored boolean value or the default value.</returns>
    public static bool GetBool(string key, bool defaultValue = default)
    {
        lock (recordsBool)
        {
            if (!recordsBool.TryGetValue(key, out var result))
            {
                result = new DataRecordBool(key, defaultValue);
                recordsBool[key] = result;
            }

            return result.GetValue();
        }
    }

    /// <summary>
    /// Saves all dirty records to PlayerPrefs. Can be delayed by specifying a frame cooldown.
    /// </summary>
    /// <param name="framesCooldown">Number of frames to wait before saving. If greater than 0,
    /// the save operation will be performed after the specified number of frames.</param>
    public static void Save(int framesCooldown = 0)
    {
        if (framesCooldown > 0)
        {
            saveProcessor.framesCooldown = framesCooldown;
            if (saveCoroutine != null)
            {
                saveProcessor.StopCoroutine(saveCoroutine);
            }

            saveCoroutine = saveProcessor.StartCoroutine(saveProcessor.SaveCoroutine(framesCooldown));
            return;
        }

        var hasChanges = false;
        lock (recordsInt)
        {
            foreach (var record in recordsInt)
            {
                hasChanges |= record.Value.isDirty;
                record.Value.Apply();
            }
        }

        lock (recordsBool)
        {
            foreach (var record in recordsBool)
            {
                hasChanges |= record.Value.isDirty;
                record.Value.Apply();
            }
        }

        lock (recordsFloat)
        {
            foreach (var record in recordsFloat)
            {
                hasChanges |= record.Value.isDirty;
                record.Value.Apply();
            }
        }

        lock (recordsString)
        {
            foreach (var record in recordsString)
            {
                hasChanges |= record.Value.isDirty;
                record.Value.Apply();
            }
        }

        lock (recordsObject)
        {
            foreach (var record in recordsObject)
            {
                hasChanges |= record.Value.isDirty;
                record.Value.Apply();
            }
        }

        if (hasChanges)
        {
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// Deletes a key and its associated value from all records and PlayerPrefs.
    /// </summary>
    /// <param name="key">The key to delete.</param>
    public static void DeleteKey(string key)
    {
        lock (recordsInt)
        {
            if (recordsInt.TryGetValue(key, out var resultInt))
            {
                resultInt.Delete();
                return;
            }
        }

        lock (recordsBool)
        {
            if (recordsBool.TryGetValue(key, out var resultBool))
            {
                resultBool.Delete();
                return;
            }
        }

        lock (recordsFloat)
        {
            if (recordsFloat.TryGetValue(key, out var resultFloat))
            {
                resultFloat.Delete();
                return;
            }
        }

        lock (recordsString)
        {
            if (recordsString.TryGetValue(key, out var resultString))
            {
                resultString.Delete();
                return;
            }
        }

        lock (recordsObject)
        {
            if (recordsObject.TryGetValue(key, out var resultObject))
            {
                resultObject.Delete();
                return;
            }
        }
    }

    /// <summary>
    /// Deletes all keys and values from all records and PlayerPrefs.
    /// </summary>
    public static void DeleteAll()
    {
        lock (recordsInt)
        {
            foreach (var record in recordsInt)
            {
                record.Value.Delete();
            }
            recordsInt.Clear();
        }

        lock (recordsBool)
        {
            foreach (var record in recordsBool)
            {
                record.Value.Delete();
            }
            recordsBool.Clear();
        }

        lock (recordsFloat)
        {
            foreach (var record in recordsFloat)
            {
                record.Value.Delete();
            }
            recordsFloat.Clear();
        }

        lock (recordsString)
        {
            foreach (var record in recordsString)
            {
                record.Value.Delete();
            }
            recordsString.Clear();
        }

        lock (recordsObject)
        {
            foreach (var record in recordsObject)
            {
                record.Value.Delete();
            }
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
            Save(0);

            // Wait for all records of objects to be processed
            lock (recordsObject)
            {
                var processed = false;
                while (!processed)
                {
                    foreach (var record in recordsObject)
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