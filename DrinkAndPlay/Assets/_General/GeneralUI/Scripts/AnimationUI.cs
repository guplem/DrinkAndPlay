using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationUI : MonoBehaviour
{
    protected bool isShowing { get; set; }
    protected AnimationCurve animationCurve { private get; set; }
    protected RectTransform rt { get; private set; }
    protected float animationDuration { private get; set; }

    protected float currentAnimTime = 0;
    private bool reachedPos;


    private void Awake()
    {
        isShowing = false;
        reachedPos = true;

        animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        animationCurve.preWrapMode = WrapMode.Clamp;
        animationCurve.postWrapMode = WrapMode.Clamp;

        rt = GetComponent<RectTransform>();

        animationDuration = 0.25f;
    }
    
    protected void StartAnim(bool isShowing)
    {
        currentAnimTime = 0f;
        reachedPos = false;
        this.isShowing = isShowing;
    }

    protected void Update()
    {
        if (reachedPos) 
            return;
        
        if (!IsAnimInEnded())
        {
            ChangeCurrentAnimTime(Time.deltaTime);
            Transition();
        }
        else
        {
            reachedPos = true;
            
            if (isShowing)
                EndAnimShowing();
            else
                EndAnimHiding();
        }
    }
    
    public abstract void Show();

    public abstract void Hide();
    
    public abstract void EndAnimShowing();

    public abstract void EndAnimHiding();

    protected abstract void Transition();

    protected float ChangeCurrentAnimTime(float deltaTime)
    {
        return currentAnimTime += deltaTime;;
    }
    
    protected float GetAnimationPosByCurve()
    {
        return GetAnimationPosByCurve(animationCurve);
    }

    protected float GetAnimationPosByCurve(AnimationCurve animCurve)
    {
        float percentageAnim = isShowing ? currentAnimTime / animationDuration : 1 - currentAnimTime / animationDuration;
        return animCurve.Evaluate(percentageAnim);
    }

    public override string ToString()
    {
        return name;
    } 

    protected bool IsAnimInEnded()
    {
        return currentAnimTime > animationDuration;
    }
    
}