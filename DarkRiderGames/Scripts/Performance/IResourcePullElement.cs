using UnityEngine;

namespace DRG
{
    public interface IResourcePullElement
    {
        void Restrain();
        void Reset();
        void Release();
        bool IsFree { get; }
        GameObject AttachedObject { get; }
    }
}
