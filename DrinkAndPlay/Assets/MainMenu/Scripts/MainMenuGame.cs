using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGame : MonoBehaviour
{
    private Section section;
    [SerializeField] private Image image;
    [SerializeField] private Localizer titleText;


    public void Setup(Section section)
    {
        this.section = section;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        ar.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        ar.aspectRatio = section.image.rect.width / section.image.rect.height;
    }

    public void SelectGame()
    {
        Debug.Log(" Section " + section + " selected.");
    }
}
