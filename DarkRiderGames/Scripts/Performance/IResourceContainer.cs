namespace DRG.Performance
{
    public interface IResourceContainer
    {
        void AddResource(IResourcePullElement element);
        void RemoveResource(IResourcePullElement element);
        void ReleaseResources();
    }
}
