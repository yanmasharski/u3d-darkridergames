using DRG;
using UnityEngine;
using DG.Tweening;

public class PresenterScale : PresenterTween, IPresenter
{

    protected override Tweener GetTweenerShow()
    {
        var tween = transform.DOScale(Vector3.one, timeShow);
        tween.SetDelay(delayShow);
        return tween;
    }

    protected override Tweener GetTweenerHide()
    {
        var tween = transform.DOScale(Vector3.zero, timeHide);
        tween.SetDelay(delayHide);
        return tween;
    }
}
