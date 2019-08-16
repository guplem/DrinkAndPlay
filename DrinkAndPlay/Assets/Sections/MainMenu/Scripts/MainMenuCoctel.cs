using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCoctel : MonoBehaviour
{
    private Cocktail cocktail;
    [SerializeField] private Image image;


    public void Setup(Cocktail cocktail)
    {
        this.cocktail = cocktail;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        //ar.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        ar.aspectRatio = cocktail.image.rect.width / cocktail.image.rect.height;

        image.sprite = cocktail.image;
    }

    public void OpenCocktail()
    {
        Debug.Log("Cocktail " + cocktail + " selected.");
    }
    
}
