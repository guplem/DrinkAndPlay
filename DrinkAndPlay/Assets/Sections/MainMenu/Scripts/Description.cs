using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsUI;

public class Description : AnimationUI
{
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject contents;


    //Default characteristics at start
    private void Start()
    {
        //TODO: closed state
    }

    private void SetElementAndPosAndSize(RectTransform rt, Vector2 position, Vector2 size)
    {
        SetAnchors(rt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), false);
        rt.position = position;
        SetAnchors(rt, new Vector2(0f, 0f), new Vector2(1f, 1f), false);
        SetSize(rt, size);
    }

    //Show animation control
    public void PlayOpenAnimationOf(Section section, GameObject originalImage)
    {
        SetOpenAnimStart(originalImage);
        SetDescriptionContents(section);
        Show();
    }
    
    public override void Show()
    {
        isShowing = true;
        StartAnim();
    }
    
    protected override bool IsShowFinished()
    {
        throw new NotImplementedException();
    }
    
    private void SetOpenAnimStart(GameObject originalImage)
    {
        //Activate elements
        image.SetActive(true);
        background.SetActive(true);
        contents.SetActive(true);
        
        //Get original image size
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        Rect rect = originalImageRect.rect;
        Vector2 imageSize = new Vector2(rect.width,rect.height);
        
        //Set all elements at start position and size
        SetElementAndPosAndSize(image.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        SetElementAndPosAndSize(background.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        SetElementAndPosAndSize(contents.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        
        //Set the start anchors' position
        SetAnchorsAroundObject(image);
        SetAnchorsAroundObject(background);
        SetAnchorsAroundObject(contents);
    }
    
    private void SetDescriptionContents(Section section)
    {
        
    }
    

    //Hide animation control
    public override void Hide()
    {
        throw new NotImplementedException();
    }

    protected override bool IsHideFinished()
    {
        throw new NotImplementedException();
    }
    
    
    //Animation itself
    protected override void Transition(float deltaTime)
    {
        //TODO
    }


}
