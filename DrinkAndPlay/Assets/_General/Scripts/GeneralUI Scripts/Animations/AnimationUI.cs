using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationUI : MonoBehaviour
{
    protected bool isShowing { get; set; }
    [SerializeField] protected AnimationCurve mainAnimationCurve;
    protected RectTransform rt { get; private set; }
    private float animationDurationOpen { get; set; }
    private float animationDurationClose { get; set; }

    private float currentAnimTime = 0;
    private bool reachedPos;


    private void Awake()
    {
        isShowing = false;
        reachedPos = true;
        
        rt = GetComponent<RectTransform>();

        animationDurationOpen = 0.20f;
        animationDurationClose = 0.15f;
    }

    public static AnimationCurve CreateLinearCurve()
    {
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(new Keyframe(0, 1));
        curve.AddKey(new Keyframe(1, 1));
        return curve;
    }
    
    protected void StartAnim(bool isShowing)
    {
        currentAnimTime = 0f;
        reachedPos = false;
        this.isShowing = isShowing;
    }

    protected void InstantHide()
    {
        currentAnimTime = animationDurationClose;
        reachedPos = false;
        this.isShowing = false;
        Update();
    }

    protected void InstantShow()
    {
        currentAnimTime = animationDurationOpen;
        reachedPos = false;
        this.isShowing = true;
        Update();
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
        return GetAnimationPosByCurve(mainAnimationCurve);
    }

    protected float GetAnimationPosByCurve(AnimationCurve animCurve)
    {
        float percentageAnim = isShowing ? GetAnimProgress(true) : 1 - GetAnimProgress(false);
        return animCurve.Evaluate(percentageAnim);
    }

    private float GetAnimProgress(bool openAnimationDuration)
    {
        if (openAnimationDuration)
            return currentAnimTime / animationDurationOpen;
        return currentAnimTime / animationDurationClose;
    }

    protected float GetUnidirectionalAnimationPosByCurve()
    {
        return GetUnidirectionalAnimationPosByCurve(mainAnimationCurve);
    }
    
    protected float GetUnidirectionalAnimationPosByCurve(AnimationCurve animCurve)
    {
        float percentageAnim = isShowing ? GetAnimProgress(true) : GetAnimProgress(false);
        return animCurve.Evaluate(percentageAnim);
    }

    public override string ToString()
    {
        return name;
    }

    private bool IsAnimInEnded()
    {
        return isShowing ? currentAnimTime > animationDurationOpen : currentAnimTime > animationDurationClose;
    }
    
}