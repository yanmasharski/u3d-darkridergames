using UnityEngine;
using System;
using System.Collections;
using Unify;

public class WWWProcessor : SingletonMonoBehaviour<WWWProcessor>
{

	
    public void SendRequest(WWW www, Action<string> onSuccess, Action<string> onFail)
    {
        StartCoroutine(StartRequest(www, onSuccess, onFail));
    }
    
    private IEnumerator StartRequest(WWW www, Action<string> onSuccess, Action<string> onFail)
    {
        yield return www;

        if (String.IsNullOrEmpty(www.error) == true)
        {
            onSuccess(www.text);
        }
        else
        {
            onSuccess(www.error);
        }
    }
}
