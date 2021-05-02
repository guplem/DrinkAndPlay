using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UtilsUI;

public class Description : AnimationUI
{

    [SerializeField] private RectTransform scrollContentsContainer;
    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject closeButtonImage;
    [SerializeField] private AnimationCurve backgroundAnimation;
    //[SerializeField] private GameObject closeButton;
    [SerializeField] private DescriptionImageHolder descriptionImageHolder;
    [SerializeField] private TextMeshProUGUI textInImage;
    [SerializeField] private AnimationCurve imageAnimation;
    [SerializeField] private GameObject contents;
    [SerializeField] private AnimationCurve contentsAnimation;
    [SerializeField] private GameObject avoidClickPanel;
    
    [SerializeField] private Localizer title;
    [SerializeField] private Localizer description;

    private RectTransform rect;
    private GameObject originalImage;
    
    #region Open caracteristics
    private Vector2 backgroundOpenAnchorMin = new Vector2(0f, 0f);
    private Vector2 backgroundOpenAnchorMax = new Vector2(1f, 1f);
    private Vector2 imageOpenAnchorMin = new Vector2(0f, 0.75f);
    private Vector2 imageOpenAnchorMax = new Vector2(1f, 0.95f);
    private Vector2 contentsOpenAnchorMin = new Vector2(0f, 0f);
    private Vector2 contentsOpenAnchorMax = new Vector2(1f, 0.75f);
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
    private RectTransform descriptionImageHolderRT;
    private RectTransform contentsRect;

    private float originalImageWidth = 0;
    private float originalTextSize = 0;

    //Default characteristics at start
    private void Start()
    {
        backgroundRect = background.GetComponent<RectTransform>();
        descriptionImageHolderRT = descriptionImageHolder.GetComponent<RectTransform>();
        contentsRect = contents.GetComponent<RectTransform>();
        rect = GetComponent<RectTransform>();
            
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;

        backgroundOpenAnchorMin = backgroundRect.anchorMin;
        backgroundOpenAnchorMax = backgroundRect.anchorMax;
        imageOpenAnchorMin = descriptionImageHolderRT.anchorMin;
        imageOpenAnchorMax = descriptionImageHolderRT.anchorMax;
        contentsOpenAnchorMin = contentsRect.anchorMin;
        contentsOpenAnchorMax = contentsRect.anchorMax;
        
        EndAnimHiding();
    }

    private void SetElementAndPosAndSize(RectTransform rt, Vector2 position, Vector2 size)
    {
        SetAnchors(rt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), false);
        rt.position = position;
        SetAnchors(rt, new Vector2(0f, 0f), new Vector2(1f, 1f), false);
        SetSize(rt, size);
    }

    //Show animation control
    public void SetupAnimationOf(string titleId, string descriptionId, GameObject originalImage, ScriptableObject cockOrSec)
    {
        SetOpenAnimStart(originalImage, cockOrSec);
        SetDescriptionContents(titleId, descriptionId);
        SaveActualPositionsAsCloseState();
        
        originalImageWidth = originalImage.GetComponent<RectTransform>().GetWidthTransform();

        Transform textChild = null;
        try {
            textChild = originalImage.transform.GetChild(0);
        } catch (Exception) { }
        
        TextMeshProUGUI originalText = null;
        if (textChild != null)
            originalText = textChild.gameObject.GetComponent<TextMeshProUGUI>();
        
        if (originalText != null)
            originalTextSize = originalText.fontSize;
        else
            originalTextSize = 0;
        
//        Debug.Log(GetAnimationPosByCurve());
        Transition();
    }

    private void SaveActualPositionsAsCloseState()
    {
        backgroundCloseAnchorMin = backgroundRect.anchorMin;
        backgroundCloseAnchorMax  = backgroundRect.anchorMax;
        
        imageCloseAnchorMin = descriptionImageHolderRT.anchorMin;
        imageCloseAnchorMax = descriptionImageHolderRT.anchorMax;

        contentsCloseAnchorMin = contentsRect.anchorMin;
        contentsCloseAnchorMax = contentsRect.anchorMax;
    }

    public override void Show()
    {
        StartAnim(true);
    }
    
    public override void EndAnimShowing() { }
    
    private void SetOpenAnimStart(GameObject originalImage, ScriptableObject cockOrSec)
    {
        this.originalImage = originalImage.gameObject;
        
        //Activate elements
        avoidClickPanel.SetActive(true);
        descriptionImageHolder.gameObject.SetActive(true);
        shadow.SetActive(true);
        background.SetActive(true);
        contents.SetActive(true);
        this.originalImage.SetActive(false);
        
        rect.SetTop(0);
        rect.SetBottom(0);

        MainMenuSection mms = descriptionImageHolder.mainMenuSection;
        if (mms != null)
            mms.Setup((Section)cockOrSec);

        else
        {
            MainMenuCoctel mmc = descriptionImageHolder.mainMenuCoctel;
            if (mmc != null)
                mmc.Setup((Cocktail)cockOrSec);
        }
        
        /*
        image.GetComponent<Image>().sprite = originalImage.GetComponent<Image>().sprite;
        
        if (textInImage != null && textInImage.gameObject.activeSelf)
            textInImage.text = originalImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        */
        
        
        
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        //Get original image size
        Rect oImgrect = originalImageRect.rect;
        Vector2 imageSize = new Vector2(oImgrect.width,oImgrect.height);
        
        //Set all elements at start position and size
        Vector3 position = originalImageRect.position;
        SetElementAndPosAndSize(descriptionImageHolderRT, position, imageSize);
        SetElementAndPosAndSize(backgroundRect, position, imageSize);
        SetElementAndPosAndSize(contentsRect, position, imageSize);
        
        //Set the start anchors' position
        SetAnchorsAroundObject(descriptionImageHolder.gameObject);
        SetAnchorsAroundObject(background);
        SetAnchorsAroundObject(contents);
    }
    
    private void SetDescriptionContents(string titleId, string descriptionId)
    {
        title.Localize(titleId);
        description.Localize(descriptionId);
    }
    

    //Hide animation control
    public override void Hide()
    {
        StartAnim(false);
    }

    public void CloseLastOpenUiElement()
    {
        GameManager.instance.generalUi.CloseLastOpenUiElement();
    }

    public void CheckIfShouldHide(Vector2 position)
    {
        if (!isShowing)
            return;

        #if UNITY_EDITOR
            if (Input.GetMouseButtonUp(0))
            {
                CheckScrollPosToHide();
            }
            
        #else 
            if (Input.touchCount <= 0) return;
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    CheckScrollPosToHide();
                    return;
            }
        
        #endif
    }

    private void CheckScrollPosToHide()
    {
        if (scrollContentsContainer.rect.height/19 < -1*scrollContentsContainer.offsetMin.y)
            CloseLastOpenUiElement();
    }
    
    public override void EndAnimHiding()
    {
        avoidClickPanel.SetActive(false);
        descriptionImageHolder.gameObject.SetActive(false);
        shadow.SetActive(false);
        background.SetActive(false);
        contents.SetActive(false);
        if (originalImage != null)
            originalImage.SetActive(true);
        
        ((MainMenuManager) SectionManager.instance).setDescIsNotOpen();
    }


    //Animation itself
    protected override void Transition()
    {
        backgroundRect.anchorMin = Vector2.Lerp(backgroundCloseAnchorMin, backgroundOpenAnchorMin,  GetAnimationPosByCurve(backgroundAnimation) );
        backgroundRect.anchorMax = Vector2.Lerp(backgroundCloseAnchorMax, backgroundOpenAnchorMax, GetAnimationPosByCurve(backgroundAnimation) );
        
        descriptionImageHolderRT.anchorMin = Vector2.Lerp(imageCloseAnchorMin, imageOpenAnchorMin, GetAnimationPosByCurve(imageAnimation));
        descriptionImageHolderRT.anchorMax = Vector2.Lerp(imageCloseAnchorMax, imageOpenAnchorMax, GetAnimationPosByCurve(imageAnimation));
        
        contentsRect.anchorMin = Vector2.Lerp(contentsCloseAnchorMin, contentsOpenAnchorMin, GetAnimationPosByCurve(backgroundAnimation));
        contentsRect.anchorMax = Vector2.Lerp(contentsCloseAnchorMax, contentsOpenAnchorMax, GetAnimationPosByCurve(backgroundAnimation));

        //Fix Buf with aspect ratio filter moving arround the image
        Vector3 pos= descriptionImageHolderRT.anchoredPosition;
        pos.x= 0f;
        descriptionImageHolderRT.anchoredPosition = pos;

        SetOpacityTo(background, Mathf.Lerp(0f, 1f, 1), false);
        SetOpacityTo(closeButtonImage, Mathf.Lerp(0f, 1f, 1), false);
        //SetOpacityTo(closeButton, GetAnimationPosByCurve(contentsAnimation), true);
        SetOpacityTo(descriptionImageHolder.gameObject, Mathf.Lerp(0f, 1f, 1), true);
        SetOpacityTo(contents, Mathf.Lerp(0f, 1f, GetAnimationPosByCurve(contentsAnimation)), true);
        //SetOpacityTo(gameObject, Mathf.Lerp(0f, 0.35f, GetAnimationPosByCurve()), false);
        SetOpacityTo(shadow, Mathf.Lerp(0f, 0.490196078f, GetAnimationPosByCurve()), false);

        if (textInImage != null)
            textInImage.fontSize = descriptionImageHolderRT.GetWidthTransform() * originalTextSize / originalImageWidth;
        
        
        //Fix close with scroll
        float topSpace = Mathf.Lerp(0, -scrollContentsContainer.GetTop(), GetUnidirectionalAnimationPosByCurve() ) ;
        rect.SetTop(topSpace);
        float bottomSpace = Mathf.Lerp(0, -scrollContentsContainer.GetBottom(), GetUnidirectionalAnimationPosByCurve() ) ;
        rect.SetBottom(bottomSpace);
    }

}
