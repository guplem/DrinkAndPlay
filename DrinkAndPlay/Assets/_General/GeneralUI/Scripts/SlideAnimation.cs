﻿using System.Collections;
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
        StartAnim(false);
    }

    public override void EndAnimShowing() { }

    public override void EndAnimHiding()  { }

    //Show animation control
    public override void Show()
    {  
        StartAnim(true);
    }

    //Animation itself
    protected override void Transition()
    {
        float animPos = GetAnimationPosByCurve();

        rt.anchorMin = Vector2.Lerp(minClose, minOpen, animPos);
        rt.anchorMax = Vector2.Lerp(maxClose, maxOpen, animPos);

        //rt.anchorMin = isShowing ? Vector2.Lerp(minClose, minOpen, animPos) : Vector2.Lerp(minOpen, minClose, animPos);
        //rt.anchorMax = isShowing ? Vector2.Lerp(maxClose, maxOpen, animPos) : Vector2.Lerp(maxOpen, maxClose, animPos);
    }
    
}
