using UnityEngine;
using DRG.Performance;

public class ResourcePullElement : MonoBehaviour, IResourcePullElement
{

    public bool IsFree { get; private set; }

    public GameObject AttachedObject
    {
        get
        {
            return gameObject;
        }
    }

    public void Restrain()
    {
        IsFree = false;
    }
    public void Reset()
    {
        // NOOP
    }

    public void Release()
    {
        IsFree = true;
    }
}
