using System;
using DRG;
using UnityEngine;
using DG.Tweening;

public abstract class PresenterTween : MonoBehaviour, IPresenter
{
    [SerializeField]
    protected float timeShow = 0.5f;

    [SerializeField]
    protected float timeHide = 0.2f;

    [SerializeField]
    protected float delayShow = 0f;

    [SerializeField]
    protected float delayHide = 0f;

    public void Hide(Action onHidden)
    {
        if (timeHide == 0)
        {
            onHidden.InvokeSafe();
            return;
        }

        GetTweenerHide().OnComplete(() =>
        {
            onHidden.InvokeSafe();
        });
    }

    public void Show(Action onShown)
    {
        if (timeShow == 0)
        {
            onShown.InvokeSafe();
            return;
        }

        GetTweenerShow().OnComplete(() =>
        {
            onShown.InvokeSafe();
        });
    }

    protected abstract Tweener GetTweenerShow();

    protected abstract Tweener GetTweenerHide();
}
