using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TurnsGame : SectionManager
{
    #region Texts
    
    private List<LocalizedText> history = new List<LocalizedText>();
    private int historyIndex = -1;

    private int minDelayForRandomChallenge = 10;
    private int maxDelayForRandomChallenge = 25;
    private int currentDelayForRandomChallenge = 0;

    public abstract void NextButton();
    public abstract void PreviousButton();
    
    protected LocalizedText GetNextText()
    {
        LocalizedText lt = null;

        historyIndex++;
        if (historyIndex == history.Count)
        {
            lt = GetRandomTextOfNaughtyLevel(GameManager.instance.dataManager.GetRandomNaughtyLevel(), true, true);
            RegisterNewTextInHistory(lt);
            ProcessRandomChallenge();
        }
        else
        {
            lt = history[historyIndex];
        }
        
        return lt;
    }
    
    
    private LocalizedText GetRandomTextOfNaughtyLevel(int naughtyLevel, bool register, bool checkNotRegistered)
    {
        while (true)
        {
            LocalizedText lt = GameManager.instance.localizationManager.GetLocalizedText(section, naughtyLevel, register, checkNotRegistered);
            if (lt == null)
            {
                Debug.Log("Localized text not found");

                if (GameManager.instance.dataManager.GetTextRegisteredQuantity(section, naughtyLevel) > 2) // To know if there are enough to remove the register of the 50%
                {
                    GameManager.instance.dataManager.RemoveOldestPercentageOfTextsRegistered(section, 25f, naughtyLevel);
                    Debug.Log("REMOVED 25%");
                }
                else
                {
                    Debug.LogWarning("There are not enough texts of level " + naughtyLevel + " for the section " + section);
                    return null;
                }
            }
            else
            {
                return lt;
            }
        }
    }

    private void RegisterNewTextInHistory(LocalizedText lt)
    {
        history.Add(lt);
        
        if (historyIndex == history.Count)
            historyIndex++;
    }

    private void ProcessRandomChallenge()
    {
        currentDelayForRandomChallenge++;
        if (currentDelayForRandomChallenge < minDelayForRandomChallenge) return;
        float probability = (currentDelayForRandomChallenge - minDelayForRandomChallenge) / (maxDelayForRandomChallenge - minDelayForRandomChallenge) * 100;
        if (probability >= Random.Range(0.0f, 100.0f))
            GameManager.instance.generalUi.ShowRandomChallenge();
    }

    
    protected LocalizedText GetPreviousText()
    {
        historyIndex--;
        
        if (historyIndex < 0)
            historyIndex = 0;
        
        return history[historyIndex];
    }

    protected LocalizedText GetCurrentText()
    {
        return history[historyIndex];
    }
    
    #endregion Texts


    #region GameFunctions

    public void Like()
    {
        //TODO: like functions
        Debug.Log("Liked text " + GetCurrentText().id);
    }

    public void AddSentence()
    {
        GameManager.instance.generalUi.OpenFeedbackMenuCurrentSection();
    }

    public void Share()
    {
        //TODO: share functions
        Debug.Log("Sharing text " + GetCurrentText().id);
        
    }

    /*public abstract void LikeButton();
    public abstract void AddSentenceButton();
    public abstract void ShareButton();*/

    #endregion
}
