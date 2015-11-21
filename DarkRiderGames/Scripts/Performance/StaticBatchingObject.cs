using UnityEngine;

namespace DRG.Performance
{
    // Place this script to the object, which should be batched
    public class StaticBatchingObject : MonoBehaviour 
    {
        [SerializeField]
        private bool RebakeOnAwake = true;
    
        [SerializeField]
        private bool RebakeOnDestroy = false;
    
        private StaticBatchingRoot StaticBatchingRoot;
    
        private void Awake()
        {
            StaticBatchingRoot = GetComponentInParent<StaticBatchingRoot>();
            StaticBatchingRoot.AddObject(this, RebakeOnAwake);
        }
    
        private void OnDestroy()
        {
            StaticBatchingRoot.RemoveObject(this, RebakeOnDestroy);
        }
    }
}