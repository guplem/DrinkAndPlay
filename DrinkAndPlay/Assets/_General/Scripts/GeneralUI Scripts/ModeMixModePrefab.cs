using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModeMixModePrefab : MonoBehaviour
{
    [SerializeField] private Localizer localizer;
    private MixModeMenu mixModeMenu;
    private LocalizationFile localizationFile;

    public void Setup(MixModeMenu mixModeMenu, LocalizationFile localizationFile, bool state)
    {
        localizer.Localize(localizationFile.ToString());
        this.mixModeMenu = mixModeMenu;
        this.localizationFile = localizationFile;
        
        //TODO: set state in toggle
    }


    public void SetState(bool newState)
    {
        mixModeMenu.SetState(localizationFile, newState);
    }
}
