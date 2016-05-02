namespace DRG.Network
{
    using System;
    using System.Collections;

    public interface IRequest
    {
        event Action<string> onSuccess;
        event Action<string> onFail;

        bool inProgress { get; }
        IEnumerator Invoke();
    }
}
