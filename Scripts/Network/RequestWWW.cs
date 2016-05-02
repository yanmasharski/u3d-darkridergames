namespace DRG.Network
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RequestWWW : IRequest
    {
        public event Action<IRequest, string> onSuccess;

        public event Action<IRequest, string> onFail;

        public bool inProgress { get; private set; }

        public WWW www { get; private set; }

        public RequestWWW(string url)
        {
            www = new WWW(url);
        }

        public RequestWWW(string url, byte[] postData, Dictionary<string, string> headers)
        {
            www = new WWW(url, postData, headers);
        }

        public IEnumerator Invoke()
        {
            inProgress = true;

            yield return www;

            inProgress = false;

            if (string.IsNullOrEmpty(www.error))onSuccess.InvokeSafe(this, www.text);
            else onFail.InvokeSafe(this, www.error);
        }
    }
}