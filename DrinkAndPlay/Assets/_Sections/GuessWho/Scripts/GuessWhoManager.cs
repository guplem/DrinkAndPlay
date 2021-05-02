using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuessWhoManager : TurnsGameManager
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
