using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationUI : MonoBehaviour
{
    public bool isShown { get; protected set; }
    protected AnimationCurve animationCurve { get; private set; }
    protected RectTransform rt { get; private set; }
    protected float animationDuration { get; private set; }

    protected float currentAnimTime = 0;


    private void Awake()
    {
        isShown = false;

        animationCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        animationCurve.preWrapMode = WrapMode.Clamp;
        animationCurve.postWrapMode = WrapMode.Clamp;

        rt = GetComponent<RectTransform>();

        animationDuration = 0.25f;
    }


    public abstract void Show();

    public abstract void Hide();

    protected abstract void Transition(float deltaTime);

    protected float GetAnimPosByCurve(float deltaTime)
    {
        currentAnimTime += deltaTime;
        return animationCurve.Evaluate(currentAnimTime / animationDuration);
    }

}
