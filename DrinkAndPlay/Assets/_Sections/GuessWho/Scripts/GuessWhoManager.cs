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
        gm.dataManager.SetTurnForNextPlayer();
        SetupTextInCard(GetNextText(false));
    }



    public override void PreviousButton()
    {
        gm.dataManager.PreviousPlayerTurn();
        SetupTextInCard(GetPreviousText());
    }
    
}
