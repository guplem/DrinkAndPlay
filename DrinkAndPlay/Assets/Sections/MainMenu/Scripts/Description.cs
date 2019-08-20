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

    private readonly Vector2 backgroundOpenAnchorMin = new Vector2(0f, 0f);
    private readonly Vector2 backgroundOpenAnchorMax = new Vector2(1f, 1f);
    private readonly Vector2 imageOpenAnchorMin = new Vector2(0f, 0.75f);
    private readonly Vector2 imageOpenAnchorMax = new Vector2(1f, 0.95f);
    private readonly Vector2 contentsOpenAnchorMin = new Vector2(0f, 0f);
    private readonly Vector2 contentsOpenAnchorMax = new Vector2(1f, 0.75f);
    #endregion
    
    #region Close caracteristics
    private Vector2 backgroundCloseAnchorMin;
    private Vector2 backgroundCloseAnchorMax;
    private Vector2 imageCloseAnchorMin;
    private Vector2 imageCloseAnchorMax;
    private Vector2 contentsCloseAnchorMin;
    private Vector2 contentsCloseAnchorMax;
    #endregion

    private RectTransform backgroundRect;
    private RectTransform imageRect;
    private RectTransform contentsRect;

    //Default characteristics at start
    private void Start()
    {
        backgroundRect = background.GetComponent<RectTransform>();
        imageRect = image.GetComponent<RectTransform>();
        contentsRect = contents.GetComponent<RectTransform>();

        //TODO: image for opacity
        
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
        backgroundCloseAnchorMin = backgroundRect.anchorMin;
        backgroundCloseAnchorMax  = backgroundRect.anchorMax;
        
        imageCloseAnchorMin = imageRect.anchorMin;
        imageCloseAnchorMax = imageRect.anchorMax;

        contentsCloseAnchorMin = contentsRect.anchorMin;
        contentsCloseAnchorMax = contentsRect.anchorMax;
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
        
        
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        //Get original image size
        Rect rect = originalImageRect.rect;
        Vector2 imageSize = new Vector2(rect.width,rect.height);
        
        //Set all elements at start position and size
        Vector3 position = originalImageRect.position;
        SetElementAndPosAndSize(imageRect, position, imageSize);
        SetElementAndPosAndSize(backgroundRect, position, imageSize);
        SetElementAndPosAndSize(contentsRect, position, imageSize);
        
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
        
        backgroundRect.anchorMin = Vector2.Lerp(backgroundCloseAnchorMin, backgroundOpenAnchorMin, animPos);
        backgroundRect.anchorMax = Vector2.Lerp(backgroundCloseAnchorMax, backgroundOpenAnchorMax, animPos);
        
        imageRect.anchorMin = Vector2.Lerp(imageCloseAnchorMin, imageOpenAnchorMin, animPos);
        imageRect.anchorMax = Vector2.Lerp(imageCloseAnchorMax, imageOpenAnchorMax, animPos);
        
        contentsRect.anchorMin = Vector2.Lerp(contentsCloseAnchorMin, contentsOpenAnchorMin, animPos);
        contentsRect.anchorMax = Vector2.Lerp(contentsCloseAnchorMax, contentsOpenAnchorMax, animPos);
        

    }

    private new void Update()
    {
        base.Update();
     
        if (Input.GetKeyDown(KeyCode.Escape))
            Hide();
    }
}
