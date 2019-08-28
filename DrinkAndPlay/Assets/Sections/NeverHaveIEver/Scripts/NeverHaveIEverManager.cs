using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NeverHaveIEverManager : SectionManager
{

    [SerializeField] private TextMeshProUGUI sentenceText;
    
    [Header("Sentence config")]
    [SerializeField] private int naughtyLevel;
    [SerializeField] private bool register;
    [SerializeField] private bool checkNotRegistered;

    public void NextSentence()
    {
        LocalizedText lt = GameManager.instance.localizationManager.GetLocalizedText(section, naughtyLevel, register, checkNotRegistered);
        if (lt == null)
            Debug.Log("lt not found");
        else
            sentenceText.text = lt.text;
    }

    public void PreviousSentence()
    {
        Debug.LogError("Not implemented yet");
    }

}
