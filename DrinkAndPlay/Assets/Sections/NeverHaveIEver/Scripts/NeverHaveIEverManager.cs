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
        sentenceText.text = lt.text;
    }

    public override void PreviousButton()
    {
        LocalizedText lt = GetPreviousText();
        sentenceText.text = lt.text;
    }
    
}
