using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
public class MainMenuGame : MonoBehaviour
{
    private Section section;
    [SerializeField] private Image image;
    [SerializeField] private Localizer titleText;


    public void Setup(Section section)
    {
        this.section = section;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        //ar.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
        ar.aspectRatio = section.image.rect.width / section.image.rect.height;

        image.sprite = section.image;
        titleText.id = section.nameId;
    }

    public void SelectGame()
    {
        Debug.Log(" Section " + section + " selected.");
    }
}
