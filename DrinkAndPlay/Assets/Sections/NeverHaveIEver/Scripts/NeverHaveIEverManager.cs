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
        {
            Debug.Log("lt not found");
            
            if (GameManager.instance.dataManager.GetTextRegisteredQuantity(section, naughtyLevel) > 2) // To know if there are enough to remove the register of the 50%
            {
                GameManager.instance.dataManager.RemoveOldestPercentageOfTextsRegistered(section, 50f, naughtyLevel);
                Debug.Log("REMOVED 50%");
                NextSentence();
            }
            else
            {
                Debug.LogWarning("There are not enough texts of level " + naughtyLevel + " for the section " + section);
            }
        }
        else
        {
            sentenceText.text = lt.text;
            return;
        }
    }

    public void PreviousSentence()
    {
        Debug.LogError("Not implemented yet");
    }

}
