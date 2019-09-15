using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSwitcher : MonoBehaviour
{
    private Image imageComponent;
    [SerializeField] private Sprite switchingSprite;
    private bool initialState = true;
    
    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    public void Switch()
    {
        Sprite oldSprite = imageComponent.sprite;
        imageComponent.sprite = switchingSprite;
        switchingSprite = oldSprite;
        initialState = !initialState;
    }

    public void SetToInitialState()
    {
        if (!initialState)
            Switch();
    }
}
