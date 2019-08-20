using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UtilsUI;

public class Description : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject contents;
    private bool isOpeningAnim = false;


    public void PlayOpenAnimation(Section section, GameObject originalImage)
    {
        SetOpenAnimStart(originalImage);
        SetDescriptionContents(section);
        isOpeningAnim = true;
    }

    private void SetOpenAnimStart(GameObject originalImage)
    {
        //Activate elements
        image.SetActive(true);
        background.SetActive(true);
        contents.SetActive(true);
        
        //Get original image size
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        Vector2 imageSize = new Vector2(originalImageRect.rect.width,originalImageRect.rect.height);
        
        //Set all elements at start position and size
        SetElementAndPosAndSize(image.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        SetElementAndPosAndSize(background.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        SetElementAndPosAndSize(contents.GetComponent<RectTransform>(), originalImage.GetComponent<RectTransform>().position, imageSize);
        
        //Set the start anchors' position
        SetAnchorsAroundObject(image);
        SetAnchorsAroundObject(background);
        SetAnchorsAroundObject(contents);
    }

    private void SetElementAndPosAndSize(RectTransform rt, Vector2 position, Vector2 size)
    {
        SetAnchors(rt, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), false);
        rt.position = position;
        SetAnchors(rt, new Vector2(0f, 0f), new Vector2(1f, 1f), false);
        SetSize(rt, size);
    }
    
    private void SetDescriptionContents(Section section)
    {
        
    }

    private void Update()
    {
        
    }
}
