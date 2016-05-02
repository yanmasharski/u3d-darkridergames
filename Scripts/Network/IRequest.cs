namespace DRG.Network
{
    using System;
    using System.Collections;

    public interface IRequest
    {
        event Action<IRequest, string> onSuccess;
        event Action<IRequest, string> onFail;

        bool inProgress { get; }
        IEnumerator Invoke();
    }
}
