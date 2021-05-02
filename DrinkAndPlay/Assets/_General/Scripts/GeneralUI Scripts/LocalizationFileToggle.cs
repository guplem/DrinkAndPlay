using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationFileToggle : MonoBehaviour
{
    [SerializeField] private Localizer localizer;
    private LocalizationFilesSelectorMenu localizationFilesSelectorMenu;
    private LocalizationFile localizationFile;

    public void Setup(LocalizationFilesSelectorMenu localizationFilesSelectorMenu, LocalizationFile localizationFile, bool state)
    {
        localizer.Localize(localizationFile.ToString());
        this.localizationFilesSelectorMenu = localizationFilesSelectorMenu;
        this.localizationFile = localizationFile;
        
        //TODO: set state in toggle
    }


    public void SetState(bool newState)
    {
        localizationFilesSelectorMenu.SetState(localizationFile, newState);
    }
}
