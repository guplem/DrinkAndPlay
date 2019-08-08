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
        //ar.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        ar.aspectRatio = coctel.image.rect.width / coctel.image.rect.height;

        image.sprite = coctel.image;
    }

    public void OpenCoctel()
    {
        Debug.Log(" Coctel " + coctel + " selected.");
    }
    
}
