using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(AspectRatioFitter))]
public class DescriptionImageHolder : MonoBehaviour
{
    
    [SerializeField] private Image image;
    private RectTransform rt;
    private AspectRatioFitter arf;
    
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        
        arf = GetComponent<AspectRatioFitter>();
        arf.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        arf.aspectRatio = image.sprite.rect.width / image.sprite.rect.height;
        
        /*
        // OLD and bad approach
         
        //rt.anchorMin = new Vector2(0.5f, rt.anchorMin.y);
        //rt.anchorMax = new Vector2(0.5f, rt.anchorMax.y);
        
        float ar = image.sprite.rect.height / image.sprite.rect.width;

        float vSize = rt.rect.height; //rt.anchorMax.y - rt.anchorMin.y;

        float hSize = vSize/ar; //vSize / ar;
        
        //Vector2 newAnchorMin = new Vector2((0.5f - hSize / 2f), rt.anchorMin.y);
        //rt.anchorMin = newAnchorMin;
        
        //Vector2 newAnchorMax = new Vector2((0.5f + hSize / 2f), rt.anchorMax.y);
        //rt.anchorMax = newAnchorMax;
        
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, hSize);
        
        Debug.Log("vSize: " + vSize + ", AR: " + ar + ", hSize: " + hSize);
        */
    }
}
