using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeverHaveIEverManager : TurnsGameManager
{
    private void Start()
    {
        NextButton();
    }

    public override void NextButton()
    {
        gm.dataManager.SetTurnForNextEnabledPlayer();
        SetupTextInCard(GetNextText(false));
    }



    public override void PreviousButton()
    {
        gm.dataManager.PreviousEnabledPlayerTurn();
        SetupTextInCard(GetPreviousText());
    }
    
}
