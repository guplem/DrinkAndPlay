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

        animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        animationCurve.preWrapMode = WrapMode.Clamp;
        animationCurve.postWrapMode = WrapMode.Clamp;

        rt = GetComponent<RectTransform>();

        animationDuration = 0.25f;

        isShowing = false;
    }
    
    protected void StartAnim()
    {
        currentAnimTime = 0f;
        reachedPos = false;
    }

    protected void Update()
    {
        if (reachedPos) 
            return;
        
        if (!IsAnimInEnded())
        {
            Transition(Time.deltaTime);
        }
        else
        {
            reachedPos = true;
        }
    }
    
    public abstract void Show();

    public abstract void Hide();

    protected abstract void Transition(float deltaTime);

    protected float GetAnimPosByCurve(float deltaTime)
    {
        currentAnimTime += deltaTime;
        return animationCurve.Evaluate(currentAnimTime / animationDuration);
    }

    public override string ToString()
    {
        return name;
    }

    protected bool IsAnimInEnded()
    {
        return IsShowFinished() || IsHideFinished();
    }

    protected abstract bool IsHideFinished();

    protected  abstract bool IsShowFinished();
}