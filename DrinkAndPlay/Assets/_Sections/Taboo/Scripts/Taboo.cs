using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taboo : TurnsGameManager
{
    
    [SerializeField] private Localizer turnText;
    [SerializeField] private GameObject[] playerTurnElements;
    [SerializeField] private GameObject[] tabooGameElements;
    
    public override void NextButton()
    {
        gm.dataManager.SetTurnForNextPlayer();
        
        if (AreWeOnTopHistory())
        {
            turnText.Localize(); //Update player

            SetActiveTurnElements(true);
            SetActiveTabooGameElements(false);
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
    
    private void SetActiveTabooGameElements(bool state)
    {
        foreach (GameObject go in tabooGameElements)
            go.SetActive(state);
    }
    
    private void SetActiveTurnElements(bool state)
    {
        foreach (GameObject go in playerTurnElements)
            go.SetActive(state);
    }
    
    public void PlayerAccepted()
    {
        SetActiveTurnElements(false);
        SetActiveTabooGameElements(true);
        
        SetupTextInCard(GetNextText(false));
    }
}
