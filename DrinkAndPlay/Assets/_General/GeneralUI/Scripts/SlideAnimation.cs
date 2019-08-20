using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAnimation : AnimationUI
{
    private readonly Vector2 minOpen = new Vector2(0, 0);
    private readonly Vector2 maxOpen = new Vector2(1, 1);
    private readonly Vector2 minClose = new Vector2(1, 0);
    private readonly Vector2 maxClose = new Vector2(2, 1);

    //Default characteristics at start
    private void Start()
    {
        rt.anchorMin = minClose;
        rt.anchorMax = maxClose;
    }

    //Hide animation control
    public override void Hide()
    {
        StartAnim();
        isShowing = false;
    }
    
    protected override bool IsHideFinished()
    {
        return !isShowing && (rt.anchorMax != maxClose || rt.anchorMin != minClose);
    }

    //Show animation control
    public override void Show()
    {
        isShowing = true;
        StartAnim();
    }
    
    protected override bool IsShowFinished()
    {
        return isShowing && (rt.anchorMax != maxOpen || rt.anchorMin != minOpen);
    }

    //Animation itself
    protected override void Transition(float deltaTime)
    {
        float animPos = GetAnimPosByCurve(deltaTime);
        rt.anchorMin = isShowing ? Vector2.Lerp(minClose, minOpen, animPos) : Vector2.Lerp(minOpen, minClose, animPos);
        rt.anchorMax = isShowing ? Vector2.Lerp(maxClose, maxOpen, animPos) : Vector2.Lerp(maxOpen, maxClose, animPos);
    }
    
}
