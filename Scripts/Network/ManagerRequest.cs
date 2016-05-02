namespace DRG.Network
{

    using System.Collections;
    using System.Collections.Generic;
    using Unify;

    public class ManagerRequest : SingletonMonoBehaviour<ManagerRequest>
    {
        private const int MAX = 3;

        private Queue<IRequest> queue = new Queue<IRequest>();
        private List<IRequest> inProgress = new List<IRequest>();

        public void SendRequest(IRequest request)
        {
            if (inProgress.Count == MAX) queue.Enqueue(request);
            else StartCoroutine(StartRequest(request));
        }

        private IEnumerator StartRequest(IRequest request)
        {
            inProgress.Add(request);
            yield return request.Invoke();
            inProgress.Remove(request);

            if (queue.Count > 0 && inProgress.Count < MAX) StartRequest(queue.Dequeue());
        }
    }
}

