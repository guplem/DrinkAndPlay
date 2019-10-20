﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCoctel : MonoBehaviour
{
    private Cocktail cocktail;
    [SerializeField] private Image image;
    [SerializeField] private Localizer titleText;


    public void Setup(Cocktail cocktail)
    {
        this.cocktail = cocktail;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        if (ar != null)
            ar.aspectRatio = cocktail.image.rect.width / cocktail.image.rect.height;
        
        image.sprite = cocktail.image;
        
        titleText.Localize(cocktail.nameId);
    }

    public void OpenCocktail()
    {
        GetComponent<ButtonAnimation>().MidAnimEvent += OpenCocktailAtEvent;
    }

    private void OpenCocktailAtEvent()
    {
        ((MainMenuManager)SectionManager.instance).OpenCocktailDescription(cocktail.nameId, cocktail.descriptionId, image.gameObject, cocktail);
        GetComponent<ButtonAnimation>().MidAnimEvent -= OpenCocktailAtEvent;
    }
    
}
