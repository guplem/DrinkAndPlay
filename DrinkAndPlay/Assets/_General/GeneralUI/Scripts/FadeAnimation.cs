using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsUI;

[System.Serializable]
public class ObjectToFade
{
    [SerializeField] public GameObject gameObject;
    [SerializeField] public bool fadeChilds = true;
    
    [Range(0f,1f)]
    [SerializeField] public float minOpacity = 0f;
    [Range(0f,1f)]
    [SerializeField] public float maxOpacity = 1f;
}

public class FadeAnimation : AnimationUI
{
    [SerializeField] private ObjectToFade[] objectsToFade;

    private void Start()
    {
        InstantHide();
    }

    public override void Show()
    {
        foreach (ObjectToFade ob in objectsToFade)
            ob.gameObject.SetActive(true);
        StartAnim(true);
    }

    public override void Hide()
    {
        StartAnim(false);
    }

    public override void EndAnimShowing()
    {

    }

    public override void EndAnimHiding()
    {
        foreach (ObjectToFade ob in objectsToFade)
            ob.gameObject.SetActive(false);
    }

    protected override void Transition()
    {
        foreach (ObjectToFade ob in objectsToFade)
        {
            SetOpacityTo(ob.gameObject, Mathf.Lerp(ob.minOpacity, ob.maxOpacity, GetAnimationPosByCurve()), ob.fadeChilds);
        }
    }
}
