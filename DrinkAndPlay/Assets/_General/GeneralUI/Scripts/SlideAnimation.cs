using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideAnimation : AnimationUI
{
    private readonly Vector2 minOpen = new Vector2(0, 0);
    private readonly Vector2 maxOpen = new Vector2(1, 1);
    private readonly Vector2 minClose = new Vector2(1, 0);
    private readonly Vector2 maxClose = new Vector2(2, 1);
    private bool reachedPos;

    private void Start()
    {
        rt.anchorMin = minClose;
        rt.anchorMax = maxClose;
    }

    public override void Hide()
    {
        currentAnimTime = 0f;
        isShown = false;
        reachedPos = false;
    }

    public override void Show()
    {
        currentAnimTime = 0f;
        isShown = true;
        reachedPos = false;
    }

    private void Update()
    {
        if (reachedPos) 
            return;
        
        if ((isShown && (rt.anchorMax != maxOpen || rt.anchorMin != minOpen)) ||
            (!isShown && (rt.anchorMax != maxClose || rt.anchorMin != minClose)))
        {
            Transition(Time.deltaTime);
        }
        else
        {
            reachedPos = true;
        }
    }

    protected override void Transition(float deltaTime)
    {
        float animPos = GetAnimPosByCurve(deltaTime);
        rt.anchorMin = isShown ? Vector2.Lerp(minClose, minOpen, animPos) : Vector2.Lerp(minOpen, minClose, animPos);
        rt.anchorMax = isShown ? Vector2.Lerp(maxClose, maxOpen, animPos) : Vector2.Lerp(maxOpen, maxClose, animPos);
    }
}
