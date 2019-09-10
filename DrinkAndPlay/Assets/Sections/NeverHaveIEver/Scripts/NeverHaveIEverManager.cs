using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeverHaveIEverManager : TurnsGame
{

    [SerializeField] private TextMeshProUGUI sentenceText;

    public override void NextButton()
    {
        LocalizedText lt = GetNextText();
        if (lt != null)
            sentenceText.text = lt.text;
    }

    public override void PreviousButton()
    {
        LocalizedText lt = GetPreviousText();
        if (lt != null)
            sentenceText.text = lt.text;
    }
    
}
