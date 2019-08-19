using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject text;
    

    public void PlayOpenAnimation(Section section, GameObject originalImage)
    {
        SetOpenAnimStart(originalImage);
        SetDescriptionContents(section);
        
    }

    private void SetOpenAnimStart(GameObject originalImage)
    {
        //Get original image size
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        Vector2 imageSize = new Vector2(originalImageRect.rect.width,originalImageRect.rect.height);
        
        //Set description image position and size
        RectTransform imageRect = image.GetComponent<RectTransform>();
        imageRect.position = originalImage.GetComponent<RectTransform>().position;
        imageRect.anchorMin = new Vector2(0f, 0f);
        imageRect.anchorMax = new Vector2(1f, 1f);
        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imageSize.x);
        imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imageSize.y);
    }
    
    private void SetDescriptionContents(Section section)
    {
        
    }

    private void Update()
    {
        
    }
}
