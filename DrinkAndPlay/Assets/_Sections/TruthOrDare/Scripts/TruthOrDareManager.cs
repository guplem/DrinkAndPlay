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
    
    private void Start()
    {
        dareLocalizationFile = section.localizationFiles[0];
        truthLocalizationFile = section.localizationFiles[1];
    }

    public override void NextButton()
    {
        gm.dataManager.NextPlayerTurn();
        
        foreach (GameObject go in chooseStepElements)
            go.SetActive(true);
    }

    public override void PreviousButton()
    {
        gm.dataManager.PreviousPlayerTurn();
        SetupTextInCard(GetPreviousText());
    }

    private void ActivateTruthOrDareElements()
    {
        foreach (GameObject go in truthOrDareElements)
            go.SetActive(true);
    }

    public void CoosenDare()
    {
        ActivateTruthOrDareElements();
        SetupTextInCard(GetNextText(truthLocalizationFile));
        
    }

    public void ChoosenTruth()
    {
        ActivateTruthOrDareElements();
        SetupTextInCard(GetNextText(truthLocalizationFile));
    }
}
