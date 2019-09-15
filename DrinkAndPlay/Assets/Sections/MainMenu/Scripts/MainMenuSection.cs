using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AspectRatioFitter))]
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

        titleText.SetId(section.nameId);
    }

    /*public void SelectGame()
    {
        Debug.Log(" Section " + section + " selected.");
    }*/

    public void OpenSectionDescription()
    {
        GetComponent<ButtonAnimation>().MidAnimEvent += OpenSectionDescriptionAtEvent;
    }

    private void OpenSectionDescriptionAtEvent()
    {
        ((MainMenuManager)SectionManager.instance).OpenSectionDescription(section, image.gameObject);
        GetComponent<ButtonAnimation>().MidAnimEvent -= OpenSectionDescriptionAtEvent;
    }
}
