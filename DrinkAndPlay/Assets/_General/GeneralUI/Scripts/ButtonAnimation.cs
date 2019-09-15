using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : AnimationUI
{
    [SerializeField] private float delayInOut = 0f;
    [SerializeField] private float movement = 0.1f;
    private IEnumerator startHideHolder;
    
    public Action MidAnimEvent;
    public Action EndAnimEvent;
    
    public override void Show()
    {
        StartAnim(true);
    }

    public override void Hide()
    {
        StartAnim(false);
    }

    public override void EndAnimShowing()
    {
        //Spread the word
        if (MidAnimEvent != null)
            MidAnimEvent();
    
        //Wait until retracting
        if (startHideHolder != null)
            StopCoroutine(startHideHolder);
        startHideHolder = StartHide();
        StartCoroutine(startHideHolder);
    }

    private IEnumerator StartHide()
    {
        yield return new WaitForSeconds(delayInOut);
        Hide();
    }

    public override void EndAnimHiding()
    {
        //Spread the word
        if (EndAnimEvent != null)
            EndAnimEvent();
    }

    protected override void Transition()
    {
        float size = Mathf.Lerp(1f, 1-movement, GetAnimationPosByCurve());
        transform.localScale = Vector3.one * size;
    }
}