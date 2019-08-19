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
        SetStartOpenAnnimationState(originalImage);
        SetDescriptionContents(section);
        
    }

    private void SetStartOpenAnnimationState(GameObject originalImage)
    {
        RectTransform originalImageRect = originalImage.GetComponent<RectTransform>();
        Vector3 localScale = originalImageRect.localScale;
        Vector2 sizeDelta = originalImageRect.sizeDelta;
        Vector2 imageSize = new Vector2(sizeDelta.x * localScale.x,sizeDelta.y * localScale.y);
        
        
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
