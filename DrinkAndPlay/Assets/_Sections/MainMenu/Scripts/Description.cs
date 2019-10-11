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
    [SerializeField] private AnimationCurve backgroundAnimation;
    [SerializeField] private GameObject closeButton;
    [SerializeField] private GameObject MainMenuCocktailOrSection;
    [SerializeField] private TextMeshProUGUI textInImage;
    [SerializeField] private AnimationCurve imageAnimation;
    [SerializeField] private GameObject contents;
    [SerializeField] private AnimationCurve contentsAnimation;
    
    [SerializeField] private Localizer title;
    [SerializeField] private Localizer description;
    
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
    private RectTransform imageRect;
    private RectTransform contentsRect;

    private float originalImageWidth = 0;
    private float originalTextSize = 0;

    //Default characteristics at start
    private void Start()
    {
        backgroundRect = background.GetComponent<RectTransform>();
        imageRect = MainMenuCocktailOrSection.GetComponent<RectTransform>();
        contentsRect = contents.GetComponent<RectTransform>();
        
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;

        backgroundOpenAnchorMin = backgroundRect.anchorMin;
        backgroundOpenAnchorMax = backgroundRect.anchorMax;
        imageOpenAnchorMin = imageRect.anchorMin;
        imageOpenAnchorMax = imageRect.anchorMax;
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
        
        Debug.Log(GetAnimationPosByCurve());
        Transition();
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
        StartAnim(true);
    }
    
    public override void EndAnimShowing() { }
    
    private void SetOpenAnimStart(GameObject originalImage, ScriptableObject cockOrSec)
    {
        //Activate elements
        MainMenuCocktailOrSection.SetActive(true);
        shadow.SetActive(true);
        background.SetActive(true);
        contents.SetActive(true);

        MainMenuSection mms = MainMenuCocktailOrSection.GetComponent<MainMenuSection>();
        if (mms != null)
        {
            mms.Setup((Section)cockOrSec);
        }
        
        else
        {
            MainMenuCoctel mmc = MainMenuCocktailOrSection.GetComponent<MainMenuCoctel>();
            if (mmc != null)
            {
                mmc.Setup((Cocktail)cockOrSec);
            }
        }
        
        /*
        image.GetComponent<Image>().sprite = originalImage.GetComponent<Image>().sprite;
        
        if (textInImage != null && textInImage.gameObject.activeSelf)
            textInImage.text = originalImage.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        */
        
        
        
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        //Get original image size
        Rect rect = originalImageRect.rect;
        Vector2 imageSize = new Vector2(rect.width,rect.height);
        
        //Set all elements at start position and size
        Vector3 position = originalImageRect.position;
        SetElementAndPosAndSize(imageRect, position, imageSize);
        SetElementAndPosAndSize(backgroundRect, position, imageSize/*0.8f*/);
        SetElementAndPosAndSize(contentsRect, position, imageSize/*0.8f*/);
        
        //Set the start anchors' position
        SetAnchorsAroundObject(MainMenuCocktailOrSection);
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
            
        #elif UNITY_ANDROID
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
        //Debug.Log((scrollContentsContainer.rect.height/8 < -1*scrollContentsContainer.offsetMin.y) + " / " + scrollContentsContainer.rect.height/8 + " < " + -1*scrollContentsContainer.offsetMin.y);
        if (scrollContentsContainer.rect.height/19 < -1*scrollContentsContainer.offsetMin.y)
            CloseLastOpenUiElement();
    }
    
    public override void EndAnimHiding()
    {
        MainMenuCocktailOrSection.SetActive(false);
        shadow.SetActive(false);
        background.SetActive(false);
        contents.SetActive(false);
    }


    //Animation itself
    protected override void Transition()
    {
        backgroundRect.anchorMin = Vector2.Lerp(backgroundCloseAnchorMin, backgroundOpenAnchorMin,  GetAnimationPosByCurve(backgroundAnimation) );
        backgroundRect.anchorMax = Vector2.Lerp(backgroundCloseAnchorMax, backgroundOpenAnchorMax, GetAnimationPosByCurve(backgroundAnimation) );
        
        imageRect.anchorMin = Vector2.Lerp(imageCloseAnchorMin, imageOpenAnchorMin, GetAnimationPosByCurve(imageAnimation));
        imageRect.anchorMax = Vector2.Lerp(imageCloseAnchorMax, imageOpenAnchorMax, GetAnimationPosByCurve(imageAnimation));
        
        contentsRect.anchorMin = Vector2.Lerp(contentsCloseAnchorMin, contentsOpenAnchorMin, GetAnimationPosByCurve(backgroundAnimation));
        contentsRect.anchorMax = Vector2.Lerp(contentsCloseAnchorMax, contentsOpenAnchorMax, GetAnimationPosByCurve(backgroundAnimation));

        SetOpacityTo(background, Mathf.Lerp(0f, 1f, 1), true);
        SetOpacityTo(closeButton, GetAnimationPosByCurve(contentsAnimation), true);
        SetOpacityTo(MainMenuCocktailOrSection, Mathf.Lerp(0f, 1f, 1), true);
        SetOpacityTo(contents, Mathf.Lerp(0f, 1f, GetAnimationPosByCurve(contentsAnimation)), true);
        //SetOpacityTo(gameObject, Mathf.Lerp(0f, 0.35f, GetAnimationPosByCurve()), false);
        SetOpacityTo(shadow, Mathf.Lerp(0f, 0.490196078f, GetAnimationPosByCurve()), false);

        if (textInImage != null)
            textInImage.fontSize = imageRect.GetWidthTransform() * originalTextSize / originalImageWidth;
    }

}
