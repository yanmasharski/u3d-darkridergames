namespace DRG.Network
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RequestWWW : IRequest
    {
        public event Action<string> onSuccess;

        public event Action<string> onFail;

        public bool inProgress { get; private set; }

        private WWW www;

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
            yield return www;

            if (string.IsNullOrEmpty(www.error))onSuccess.InvokeSafe(www.text);
            else onFail.InvokeSafe(www.error);
        }
    }
}