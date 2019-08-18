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

        titleText.SetId(section.nameId);
    }

    /*public void SelectGame()
    {
        Debug.Log(" Section " + section + " selected.");
    }*/

    public void OpenSectionDescription()
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector3 localScale = rt.localScale;
        Vector2 sizeDelta = rt.sizeDelta;
        Vector2 imageSize = new Vector2(sizeDelta.x * localScale.x,sizeDelta.y * localScale.y);
        ((MainMenuManager)SectionManager.instance).OpenSectionDescription(section, image.gameObject, imageSize);
    }
}
