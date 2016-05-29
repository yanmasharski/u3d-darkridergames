namespace DRG
{
    using System;

    public interface IPresenter
    {
        void Show(Action onShown);
        void Hide(Action onHidden);
    }
}