using UnityEngine;

namespace DRG.Performance
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
