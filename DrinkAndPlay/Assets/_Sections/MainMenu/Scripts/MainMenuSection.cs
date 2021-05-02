using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(AspectRatioFitter))]
public class MainMenuSection : MonoBehaviour
{
    private Section section;
    [SerializeField] private Image image;
    [SerializeField] private Localizer titleText;
    [SerializeField] private GameObject commingSoon;

    public void Setup(Section section)
    {
        this.section = section;

        AspectRatioFitter ar = GetComponent<AspectRatioFitter>();
        if (ar != null)
        {
            ar.aspectRatio = section.image.rect.width / section.image.rect.height;

            AspectRatioFitter parentAr = transform.parent.GetComponent<AspectRatioFitter>();
            if (parentAr != null)
                parentAr.aspectRatio = ar.aspectRatio*2;
        }

        
        image.sprite = section.image;
        
        commingSoon.SetActive(section.comingSoon);

        titleText.Localize(section.nameId);
    }

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
