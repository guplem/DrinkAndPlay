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
    
    #region Open caracteristics
    Vector2 backgroundOpenAnchorMin = new Vector2(0f, 0f);
    Vector2 backgroundOpenAnchorMax = new Vector2(1f, 1f);
    Vector2 imageOpenAnchorMin = new Vector2(0f, 0.75f);
    Vector2 imageOpenAnchorMax = new Vector2(1f, 1f);
    Vector2 contentsOpenAnchorMin = new Vector2(0f, 0f);
    Vector2 contentsOpenAnchorMax = new Vector2(1f, 0.75f);
    #endregion
    
    #region Close caracteristics
    private Vector2 backgroundCloseAnchorMin;
    private Vector2 backgroundCloseAnchorMax;
    private Vector2 imageCloseAnchorMin;
    private Vector2 imageCloseAnchorMax;
    private Vector2 contentsCloseAnchorMin;
    private Vector2 contentsCloseAnchorMax;
    #endregion


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
        SaveActualPositionsAsCloseState();
        SetDescriptionContents(section);
        Show();
    }

    private void SaveActualPositionsAsCloseState()
    {
        backgroundCloseAnchorMin = background.GetComponent<RectTransform>().anchorMin;
        backgroundCloseAnchorMax  = background.GetComponent<RectTransform>().anchorMax;
        
        imageCloseAnchorMin = image.GetComponent<RectTransform>().anchorMin;
        imageCloseAnchorMax = image.GetComponent<RectTransform>().anchorMax;

        contentsCloseAnchorMin = contents.GetComponent<RectTransform>().anchorMin;
        contentsCloseAnchorMax = contents.GetComponent<RectTransform>().anchorMax;
    }

    public override void Show()
    {
        isShowing = true;
        StartAnim();
    }
    
    protected override bool IsShowFinished()
    {
        //TODO: check if the open/show anim is finished
        return false;
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
        isShowing = false;
        StartAnim();
    }

    protected override bool IsHideFinished()
    {
        //TODO: check if the close/hide anim is finished
        return false;
    }
    
    
    //Animation itself
    protected override void Transition(float deltaTime)
    {
        float animPos = GetAnimPosByCurve(deltaTime);

        animPos = isShowing ? animPos : 1 - animPos;
        
        background.GetComponent<RectTransform>().anchorMin = Vector2.Lerp(backgroundCloseAnchorMin, backgroundOpenAnchorMin, animPos);
        background.GetComponent<RectTransform>().anchorMax = Vector2.Lerp(backgroundCloseAnchorMax, backgroundOpenAnchorMax, animPos);
        
        image.GetComponent<RectTransform>().anchorMin = Vector2.Lerp(imageCloseAnchorMin, imageOpenAnchorMin, animPos);
        image.GetComponent<RectTransform>().anchorMax = Vector2.Lerp(imageCloseAnchorMax, imageOpenAnchorMax, animPos);
        
        contents.GetComponent<RectTransform>().anchorMin = Vector2.Lerp(contentsCloseAnchorMin, contentsOpenAnchorMin, animPos);
        contents.GetComponent<RectTransform>().anchorMax = Vector2.Lerp(contentsCloseAnchorMax, contentsOpenAnchorMax, animPos);
        

    }

    private new void Update()
    {
        base.Update();
     
        if (Input.GetKeyDown(KeyCode.Escape))
            Hide();
    }
}
