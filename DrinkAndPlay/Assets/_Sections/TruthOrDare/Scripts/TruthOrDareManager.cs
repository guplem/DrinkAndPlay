using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruthOrDareManager : TurnsGameManager
{
    private LocalizationFile dareLocalizationFile;
    private LocalizationFile truthLocalizationFile;

    [SerializeField] private GameObject[] chooseStepElements;
    [SerializeField] private GameObject[] truthOrDareElements;
    [SerializeField] private Localizer chooseText;
    
    private void Start()
    {
        dareLocalizationFile = section.localizationFiles[0];
        truthLocalizationFile = section.localizationFiles[1];
    }

    public override void NextButton()
    {
        gm.dataManager.SetTurnForNextPlayer();
        
        if (AreWeOnTopHistory())
        {
            chooseText.Localize(); //Update player

            SetActivateChooseStepElements(true);
            SetActivateTruthOrDareElements(false);
        }
        else
        {
            SetupTextInCard(GetNextText(false));
        }
        
    }

    public override void PreviousButton()
    {
        gm.dataManager.PreviousPlayerTurn();
        SetupTextInCard(GetPreviousText());
    }

    private void SetActivateTruthOrDareElements(bool state)
    {
        foreach (GameObject go in truthOrDareElements)
            go.SetActive(state);
    }
    
    private void SetActivateChooseStepElements(bool state)
    {
        foreach (GameObject go in chooseStepElements)
            go.SetActive(state);
    }

    public void CoosenDare()
    {
        
        SetActivateChooseStepElements(false);
        SetActivateTruthOrDareElements(true);
        SetupTextInCard(GetNextText(dareLocalizationFile));
        
    }

    public void ChoosenTruth()
    {
        
        SetActivateChooseStepElements(false);
        SetActivateTruthOrDareElements(true);
        SetupTextInCard(GetNextText(truthLocalizationFile));
    }
}
