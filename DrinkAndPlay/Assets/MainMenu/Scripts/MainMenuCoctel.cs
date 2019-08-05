using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCoctel : MonoBehaviour
{
    private Coctel coctel;
    [SerializeField] private Image image;


    public void Setup(Coctel coctel)
    {
        this.coctel = coctel;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        ar.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        ar.aspectRatio = coctel.image.rect.width / coctel.image.rect.height;

        image.sprite = coctel.image;
    }

    public void SelectCoctel()
    {
        Debug.Log(" Coctel " + coctel + " selected.");
    }
}
