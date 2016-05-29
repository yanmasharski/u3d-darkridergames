using DRG;
using UnityEngine;
using DG.Tweening;

[RequireComponent (typeof(CanvasGroup))]
public class PresenterFade : PresenterTween, IPresenter
{
    private CanvasGroup canvasGroup;

    protected override Tweener GetTweenerShow()
    {
        var tween = canvasGroup.DOFade(1, timeShow);
        tween.SetDelay(delayShow);
        return tween;
    }

    protected override Tweener GetTweenerHide()
    {
        var tween = canvasGroup.DOFade(0, timeHide);
        tween.SetDelay(delayHide);
        return tween;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
