using System;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// Provides a mechanism to execute code on Unity's main thread from any thread.
/// This is essential for operations that must interact with Unity's API, which is not thread-safe.
/// </summary>
public static class MainThreadDispatcher
{
    /// <summary>
    /// Thread-safe queue of actions to be executed on the main thread.
    /// </summary>
    private static readonly ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();
    
    /// <summary>
    /// MonoBehaviour instance that processes the queued actions during Update.
    /// </summary>
    private static MainThreadDispatcherBehaviour instance;

    /// <summary>
    /// Static constructor that initializes the dispatcher when the class is first accessed.
    /// Creates a persistent GameObject with the MainThreadDispatcherBehaviour component.
    /// </summary>
    static MainThreadDispatcher()
    {
        instance = new GameObject("MainThreadDispatcher").AddComponent<MainThreadDispatcherBehaviour>();
        GameObject.DontDestroyOnLoad(instance.gameObject);
    }

    /// <summary>
    /// Queues an action to be executed on the main thread.
    /// This method is thread-safe and can be called from any thread.
    /// </summary>
    /// <param name="action">The action to execute on the main thread. If null, the action is ignored.</param>
    public static void Enqueue(Action action)
    {
        if (action == null)
        {
            return;
        }
        actions.Enqueue(action);
    }

    /// <summary>
    /// MonoBehaviour that processes the queued actions during each Update call.
    /// </summary>
    private class MainThreadDispatcherBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Executes all queued actions on the main thread.
        /// Catches and logs any exceptions that occur during execution.
        /// </summary>
        private void Update()
        {
            while (actions.TryDequeue(out var action))
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }
    }
}
