using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeverHaveIEverManager : TurnsGameManager
{

    [SerializeField] private Localizer sentenceText;

    private void Start()
    {
        NextButton();
    }

    public override void NextButton()
    {
        SetupText(GetNextTextId());
    }



    public override void PreviousButton()
    {
        SetupText(GetPreviousTextId());
    }
    
    
    private void SetupText(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("The obtained ID for the card is null or empty.", gameObject);    
            return;
        }
        
        sentenceText.Localize(id);

        likeButton.SetToInitialState();
        
        if (IsCurrentTextLiked())
            likeButton.Switch();
    }
}
