using UnityEngine;
using System.Collections.Generic;

namespace drg
{
    // Place this script to the object, which should be the root for batching objects group
    public class StaticBatchingRoot : MonoBehaviour
    {
    
        private List<GameObject> BatchingList = new List<GameObject>();
        private bool RebakeNeeded = false;
    
        public void AddObject(StaticBatchingObject obj, bool rebakeNeeded)
        {
            BatchingList.Add(obj.gameObject);
    
            if (RebakeNeeded == false)
                RebakeNeeded = rebakeNeeded;
        }
    
        public void RemoveObject(StaticBatchingObject obj, bool rebakeNeeded)
        {
            BatchingList.Remove(obj.gameObject);
    
            if (RebakeNeeded == false)
                RebakeNeeded = rebakeNeeded;
        }
    
        private void Update()
        {
            if (RebakeNeeded == true)
            {
                RebakeNeeded = false;
                StaticBatchingUtility.Combine(BatchingList.ToArray(), this.gameObject);
            }
        }
        
    }
}