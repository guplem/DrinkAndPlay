using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taboo : TurnsGameManager
{
    [SerializeField] private Localizer turnText;
    [SerializeField] private GameObject[] playerTurnElements;
    [SerializeField] private GameObject[] tabooGameElements;
    [SerializeField] private Timer timer;

    private void Start()
    {
        NextButton();
    }

    public override void NextButton()
    {
        if (PlayingWithPlayers())
            gm.dataManager.SetTurnForNextEnabledPlayer();
        
        if (AreWeOnTopHistory())
        {
            if (PlayingWithPlayers()) 
            {
                turnText.Localize(); //Update player

                SetActiveTurnElements(true);
                SetActiveTabooGameElements(false);
            }
            else
            {
                PlayerAccepted();
            }
        }
        else
        {
            SetupTextInCard(GetNextText(false));
            SetTimer();
        }
    }

    private bool PlayingWithPlayers()
    {
        return (GameManager.instance.dataManager.GetEnabledPlayersQuantity() >= 2);
    }

    public override void PreviousButton()
    {
        gm.dataManager.PreviousEnabledPlayerTurn();
        SetupTextInCard(GetPreviousText());
        SetTimer();
    }

    private void SetTimer()
    {
        // Handeled by the localizer/text, no need to force it
        
        // timer.SetTimerReadyFor(challengeDuration);
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
        SetTimer();
    }
}
