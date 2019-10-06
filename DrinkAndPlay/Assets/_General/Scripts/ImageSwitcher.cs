using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageSwitcher : MonoBehaviour
{
    private bool initialState = true;
    
    private Image imageComponent;
    [SerializeField] private Sprite switchingSprite;

    private ColorSwitcher colorSwitcher;
    [SerializeField] private LightDarkColor switchingColor;


    private void Start()
    {
        imageComponent = GetComponent<Image>();
        colorSwitcher = GetComponent<ColorSwitcher>();
    }

    public void Switch()
    {
        Sprite oldSprite = imageComponent.sprite;
        imageComponent.sprite = switchingSprite;
        switchingSprite = oldSprite;

        if (colorSwitcher != null && switchingColor != null)
        {
            LightDarkColor oldColor = colorSwitcher.lightDarkColor;
            colorSwitcher.lightDarkColor = switchingColor;
            switchingColor = oldColor;
            colorSwitcher.UpdateColor();
        }

        initialState = !initialState;
    }

    public void SetToInitialState()
    {
        if (!initialState)
            Switch();
    }
}
