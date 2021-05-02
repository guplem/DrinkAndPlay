using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionDescription : Description
{
    [SerializeField] private Button playButton;
    [SerializeField] private Localizer playText;
    
    public new void SetupAnimationOf(string titleId, string descriptionId, GameObject originalImage, ScriptableObject cockOrSec)
    {
        base.SetupAnimationOf(titleId, descriptionId, originalImage, cockOrSec);
        
        Section sec = null;
        try
        {
            sec = (Section) cockOrSec;
        }
        catch (InvalidCastException)
        {
            Debug.LogWarning("Trying to use a cocktail as a section for a description.");
        }

        if (sec != null)
        {
            playButton.interactable = !sec.comingSoon;
            playText.Localize(sec.comingSoon ? "ComingSoon" : "Play");
        }

    }
}
