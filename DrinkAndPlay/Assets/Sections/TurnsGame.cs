using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInTurnsGame
{
    public LocalizedText localizedText;
    public bool liked;

    public TextInTurnsGame(LocalizedText localizedText)
    {
        this.localizedText = localizedText;
        this.liked = false;
    }

    public TextInTurnsGame(LocalizedText localizedText, bool liked)
    {
        this.localizedText = localizedText;
        this.liked = liked;
    }
}

public abstract class TurnsGame : SectionManager
{
    
    
    [SerializeField] protected ImageSwitcher likeButton;
    
    #region Texts
    
    private List<TextInTurnsGame> history = new List<TextInTurnsGame>();
    private int historyIndex = -1;

    private int minDelayForRandomChallenge = 10;
    private int maxDelayForRandomChallenge = 25;
    private int currentDelayForRandomChallenge = 0;
    
    
    private int minLikesToRatePopup = 5;
    private float minPercentageToRatePopup = 30f;

    public abstract void NextButton();
    public abstract void PreviousButton();
    
    protected LocalizedText GetNextText()
    {
        LocalizedText lt = null;

        historyIndex++;
        if (historyIndex == history.Count)
        {
            lt = GetRandomText(true, true);
            RegisterNewTextInHistory(lt);
            ProcessRandomChallenge();
        }
        else
        {
            lt = GetCurrentText();
        }
        
        return lt;
    }
    
    
    private LocalizedText GetRandomText(bool register, bool checkNotRegistered)
    {
        while (true)
        {
            LocalizedText lt = GameManager.instance.localizationManager.GetLocalizedText(section, register, checkNotRegistered);
            if (lt == null)
            {
                Debug.Log("Localized text not found");

                if (GameManager.instance.dataManager.GetTextRegisteredQuantity(section) > 2) // To know if there are enough to remove the register of the 50%
                {
                    GameManager.instance.dataManager.RemoveOldestPercentageOfTextsRegistered(section, 25f);
                    Debug.Log("REMOVED 25%");
                    GameManager.instance.localizationManager.RandomizeLocalizedTexts(section); // To change the order of the new avaliable texts
                }
                else
                {
                    Debug.LogWarning("There are not enough texts for the section " + section);
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
        history.Add(new TextInTurnsGame(lt));
        
        if (historyIndex == history.Count)
            historyIndex++;
    }

    private void ProcessRandomChallenge()
    {
        currentDelayForRandomChallenge++;
        if (currentDelayForRandomChallenge < minDelayForRandomChallenge) return;
        float probability = (currentDelayForRandomChallenge - minDelayForRandomChallenge) / (maxDelayForRandomChallenge - minDelayForRandomChallenge) * 100;
        if (probability >= Random.Range(0.0f, 100.0f))
        {
            GameManager.instance.generalUi.ShowRandomChallenge();
            currentDelayForRandomChallenge = 0;
        }
    }

    
    protected LocalizedText GetPreviousText()
    {
        if (history.Count <= 0)
            return null;
        
        historyIndex--;
        
        if (historyIndex < 0)
            historyIndex = 0;

        return GetCurrentText();
    }

    protected LocalizedText GetCurrentText()
    {
        if (historyIndex < 0)
            return null;
        
        return history[historyIndex].localizedText;
    }
    
    #endregion Texts


    #region GameFunctions

    public void Like()
    {
        history[historyIndex].liked = !history[historyIndex].liked;
        likeButton.Switch();

        // Check if the rate popup should be shown
        
        if (history.Count < minLikesToRatePopup)    
            return;

        int likeCount = 0;
        foreach (TextInTurnsGame txt in history)
            if (txt.liked)
                likeCount++;
        
        if (likeCount < minLikesToRatePopup)
            return;

        float percentage = likeCount * 100f / history.Count;
        if (percentage < minPercentageToRatePopup)
            return;

        if (gm.dataManager.ratePopupShown || gm.dataManager.ratedApp)
            return;
        
        gm.generalUi.ShowRatePopup();
    }

    public bool IsCurrentTextLiked()
    {
        return history[historyIndex].liked;
    }

    public void AddSentence()
    {
        GameManager.instance.generalUi.OpenFeedbackMenuCurrentSection();
    }

    public void Share()
    {
        //TODO: share functions
        Debug.Log("Sharing text.");
        
    }

    /*public abstract void LikeButton();
    public abstract void AddSentenceButton();
    public abstract void ShareButton();*/
    
    #endregion
}
