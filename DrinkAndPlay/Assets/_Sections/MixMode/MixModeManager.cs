using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MixModeManager : TurnsGameManager
{
    private void Start()
    {
        if (!gm.dataManager.IsSelectedLocalizationFilesListInitialized())
        {
//            Debug.LogWarning("The list of the selected localization files was not initialized. (It is normal if this scene is the first to run).");
            gm.dataManager.SetSelectedLocalizationFiles(section.localizationFiles.ToList());
        }
        
        if (gm.dataManager.GetSelectedLocalizationFilesQuantity() <= 0)
        {
            Debug.LogWarning("The list of the selected localization files does not have enough selected (" + gm.dataManager.GetSelectedLocalizationFilesQuantity() + ").\nPerforming an autimatic selection of all the options.");
            gm.dataManager.SetSelectedLocalizationFiles(section.localizationFiles.ToList());
        }
        
        NextButton();
    }
    
    public override void NextButton()
    {
        gm.dataManager.SetTurnForNextEnabledPlayer();
        TextInTurnsGame nextText = GetNextText(true);
        SetupTextInCard(nextText);
        if (section.topBar)
            GameManager.instance.generalUi.sectionTitleLocalizer.Localize(nextText.localizationFile.ToString());
        else
            Debug.LogWarning("The top bar in the UI should be enabled for the 'Mix mode' section: " + section,gameObject);
    }
    
    public override void PreviousButton()
    {
        gm.dataManager.PreviousEnabledPlayerTurn();
        SetupTextInCard(GetPreviousText());
    }


}
