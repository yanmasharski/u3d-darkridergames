using UnityEngine;
using System;
using System.Collections;
using Unify;

namespace DRG.Network
{
    public class WWWProcessor : SingletonMonoBehaviour<WWWProcessor>
    {

        public void SendRequest(WWW www, Action<string> onSuccess = null, Action<string> onFail = null)
        {
            StartCoroutine(StartRequest(www, onSuccess, onFail));
        }

        private IEnumerator StartRequest(WWW www, Action<string> onSuccess, Action<string> onFail)
        {
            yield return www;

            if (String.IsNullOrEmpty(www.error) == true)
            {
                if (onSuccess!= null)
                {
                    onSuccess(www.text);
                }
            }
            else
            {
                if (onFail != null)
                {
                    onFail(www.error);
                }
            }
        }
    }
}

