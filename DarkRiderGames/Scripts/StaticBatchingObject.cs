using UnityEngine;
using System.Collections;

// Place this script to the object, which should be batched
public class StaticBatchingObject : MonoBehaviour 
{
    [SerializeField]
    private bool RebakeOnAwake = true;

    [SerializeField]
    private bool RebakeOnDestroy = false;

    private StaticBatching StaticBatching;

	private void Awake()
    {
        StaticBatching = GetComponentInParent<StaticBatching>();
        StaticBatching.AddObject(this, RebakeOnAwake);
    }

    private void OnDestroy()
    {
        StaticBatching.RemoveObject(this, RebakeOnDestroy);
    }
}
