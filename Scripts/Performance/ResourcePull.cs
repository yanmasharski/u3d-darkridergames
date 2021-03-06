﻿using UnityEngine;
using System.Collections.Generic;
using Unify;

namespace DRG.Performance
{
    public class ResourcePull : SingletonMonoBehaviour<ResourcePull>
    {
        private Dictionary<Object, List<IResourcePullElement>> Pull = new Dictionary<Object, List<IResourcePullElement>>();
        private Dictionary<string, Object> ResourcePathDictionary = new Dictionary<string, Object>();

        public T GetInstance<T>(string path) where T : Component
        {
            return (GetInstance(path) as GameObject).GetComponent<T>();
        }

        public T GetInstance<T>(Object prefab) where T : Component
        {
            return (GetInstance(prefab) as GameObject).GetComponent<T>();
        }

        public Object GetInstance(string path)
        {
            Object prefab;

            if (ResourcePathDictionary.TryGetValue(path, out prefab) == false) 
            {
                prefab = Resources.Load(path);
                ResourcePathDictionary.Add(path, prefab);
            }

            return GetInstance(prefab);
        }

        public Object GetInstance(Object prefab)
        {
            List<IResourcePullElement> prefabPull;

            if (Pull.TryGetValue(prefab, out prefabPull) == false)
            {
                prefabPull = new List<IResourcePullElement>();
                Pull.Add(prefab, prefabPull);
            }

            IResourcePullElement instance = prefabPull.Find((element => element.IsFree == true));

            if (instance == null)
            {
                instance = (Instantiate(prefab) as GameObject).GetComponent<IResourcePullElement>();
                prefabPull.Add(instance);
            }

            instance.Restrain();
            return instance.AttachedObject;
        }

        public void ReleaseInstance(IResourcePullElement element)  
        {
            element.AttachedObject.SetActive(false);
            element.AttachedObject.transform.SetParent(transform);
        }
        
    }
}