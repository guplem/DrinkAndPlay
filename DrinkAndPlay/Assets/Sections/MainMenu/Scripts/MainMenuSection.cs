using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSection : MonoBehaviour
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

        titleText.Localize(section.nameId);
    }

    public void SelectGame()
    {
        Debug.Log(" Section " + section + " selected.");
    }

    public void OpenSectionDescription()
    {
        ((MainMenuManager)SectionManager.Instance).OpenSectionDescription(section);
    }
}
