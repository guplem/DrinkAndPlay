using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeverHaveIEverManager : TurnsGame
{

    [SerializeField] private Localizer sentenceText;

    public override void NextButton()
    {
        SetupText(GetNextText());
    }



    public override void PreviousButton()
    {
        SetupText(GetPreviousText());
    }
    
    
    private void SetupText(LocalizedText lt)
    {
        if (lt != null)
            sentenceText.SetText(lt);

        likeButton.SetToInitialState();
        
        if (IsCurrentTextLiked())
            likeButton.Switch();
    }
}
